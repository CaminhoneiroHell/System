using QFSW.QC.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace QFSW.QC
{
    /// <summary>
    /// Provides the UI and I/O interface for the QuantumConsoleProcessor. Invokes commands on the processor and displays the output.
    /// </summary>
    [DisallowMultipleComponent]
    public class QuantumConsole : MonoBehaviour
    {
        /// <summary>
        /// Singleton reference to the console. Only valid and set if the singleton option is enabled for the console.
        /// </summary>
        public static QuantumConsole Instance { get; private set; }

#pragma warning disable 0414, 0067, 0649
        [FormerlySerializedAs("containerRect")]
        [SerializeField] private RectTransform _containerRect;
        [FormerlySerializedAs("scrollRect")]
        [SerializeField] private ScrollRect _scrollRect;
        [FormerlySerializedAs("suggestionPopupRect")]
        [SerializeField] private RectTransform _suggestionPopupRect;
        [FormerlySerializedAs("jobCounterRect")]
        [SerializeField] private RectTransform _jobCounterRect;
        [FormerlySerializedAs("panels")]
        [SerializeField] private Image[] _panels;

        [FormerlySerializedAs("theme")]
        [SerializeField] private QuantumTheme _theme;
        [SerializeField] private QuantumKeyConfig _keyConfig;

        [FormerlySerializedAs("verboseErrors")]
        [Command("verbose-errors", "If errors caused by the Quantum Console Processor or commands should be logged in verbose mode.", MonoTargetType.Registry)]
        [SerializeField] private bool _verboseErrors = false;

        [FormerlySerializedAs("verboseLogging")]
        [Command("verbose-logging", "The minimum log severity required to use verbose logging.", MonoTargetType.Registry)]
        [SerializeField] private LoggingThreshold _verboseLogging = LoggingThreshold.Never;
        [FormerlySerializedAs("openOnLogLevel")]
        [SerializeField] private LoggingThreshold _openOnLogLevel = LoggingThreshold.Never;
        [FormerlySerializedAs("interceptDebugLogger")]
        [SerializeField] private bool _interceptDebugLogger = true;
        [FormerlySerializedAs("interceptWhilstInactive")]
        [SerializeField] private bool _interceptWhilstInactive = true;
        [FormerlySerializedAs("prependTimestamps")]
        [SerializeField] private bool _prependTimestamps = false;

        [FormerlySerializedAs("supportedState")]
        [SerializeField] private SupportedState _supportedState = SupportedState.Always;
        [FormerlySerializedAs("activateOnStartup")]
        [SerializeField] private bool _activateOnStartup = true;
        [FormerlySerializedAs("initialiseOnStartup")]
        [SerializeField] private bool _initialiseOnStartup = false;
        [FormerlySerializedAs("closeOnSubmit")]
        [SerializeField] private bool _closeOnSubmit = false;
        [FormerlySerializedAs("dontDestroyOnLoad")]
        [SerializeField] private bool _singletonMode = false;
        [FormerlySerializedAs("autoScroll")]
        [SerializeField] private AutoScrollOptions _autoScroll = AutoScrollOptions.OnInvoke;

        [FormerlySerializedAs("showPopupDisplay")]
        [SerializeField] private bool _showPopupDisplay = true;
        [FormerlySerializedAs("suggestionDisplayOrder")]
        [SerializeField] private SortOrder _suggestionDisplayOrder = SortOrder.Descending;
        [FormerlySerializedAs("maxSuggestionDisplaySize")]
        [SerializeField] private int _maxSuggestionDisplaySize = -1;
        [FormerlySerializedAs("useFuzzySearch")]
        [SerializeField] private bool _useFuzzySearch = false;
        [FormerlySerializedAs("caseSensitiveSearch")]
        [SerializeField] private bool _caseSensitiveSearch = true;

        [FormerlySerializedAs("showCurrentJobs")]
        [SerializeField] private bool _showCurrentJobs = true;
        [FormerlySerializedAs("blockOnAsync")]
        [SerializeField] private bool _blockOnAsync = false;

        [FormerlySerializedAs("storeCommandHistory")]
        [SerializeField] private bool _storeCommandHistory = true;
        [FormerlySerializedAs("storeDuplicateCommands")]
        [SerializeField] private bool _storeDuplicateCommands = true;
        [FormerlySerializedAs("storeAdjacentDuplicateCommands")]
        [SerializeField] private bool _storeAdjacentDuplicateCommands = false;
        [FormerlySerializedAs("commandHistorySize")]
        [SerializeField] private int _commandHistorySize = -1;

        [FormerlySerializedAs("maxStoredLogs")]
        [SerializeField] private int _maxStoredLogs = 100;
        [FormerlySerializedAs("showInitLogs")]
        [SerializeField] private bool _showInitLogs = true;

        [FormerlySerializedAs("consoleInputTMP")]
        [SerializeField] private TMP_InputField _consoleInput;
        [FormerlySerializedAs("inputPlaceholderTextTMP")]
        [SerializeField] private TextMeshProUGUI _inputPlaceholderText;
        [FormerlySerializedAs("consoleLogTextTMP")]
        [SerializeField] private TextMeshProUGUI _consoleLogText;
        [FormerlySerializedAs("consoleSuggestionTextTMP")]
        [SerializeField] private TextMeshProUGUI _consoleSuggestionText;
        [FormerlySerializedAs("suggestionPopupTextTMP")]
        [SerializeField] private TextMeshProUGUI _suggestionPopupText;
        [FormerlySerializedAs("jobCounterTextTMP")]
        [SerializeField] private TextMeshProUGUI _jobCounterText;
#pragma warning restore 0414, 0067, 0649

        #region Callbacks
        /// <summary>Callback executed when the QC state changes.</summary>
        public event Action OnStateChange;

        /// <summary>Callback executed when the QC invokes a command.</summary>
        public event Action<string> OnInvoke;

        /// <summary>Callback executed when the QC is cleared.</summary>
        public event Action OnClear;

        /// <summary>Callback executed when text has been logged to the QC.</summary>
        public event Action<string> OnLog;

        /// <summary>Callback executed when the QC is activated.</summary>
        public event Action OnActivate;

        /// <summary>Callback executed when the QC is deactivated.</summary>
        public event Action OnDeactivate;
        #endregion

        private bool IsBlockedByAsync => _blockOnAsync && _currentTasks.Count > 0;

        private readonly QuantumSerializer _serializer = new QuantumSerializer();

        private readonly List<string> _consoleLogs = new List<string>(10);
        private readonly StringBuilder _logTraceBuilder = new StringBuilder(2048);
        private readonly ConcurrentQueue<Log> _queuedLogs = new ConcurrentQueue<Log>();

        public bool IsActive { get; private set; }

        private readonly List<string> _previousCommands = new List<string>();
        private readonly List<Task> _currentTasks = new List<Task>();
        private readonly List<CommandData> _suggestedCommands = new List<CommandData>();
        private int _selectedPreviousCommandIndex = -1;
        private int _selectedSuggestionCommandIndex = -1;
        private string _currentText;
        private string _previousText;
        private bool _isGeneratingTable;
        private bool _consoleRequiresFlush;

        private TextMeshProUGUI[] _textComponents;

        private readonly Type _voidTaskType = typeof(Task<>).MakeGenericType(Type.GetType("System.Threading.Tasks.VoidTaskResult"));

        /// <summary>Applies a theme to the Quantum Console.</summary>
        /// <param name="theme">The desired theme to apply.</param>
        public void ApplyTheme(QuantumTheme theme, bool forceRefresh = false)
        {
            _theme = theme;
            if (theme)
            {
                if (_textComponents == null || forceRefresh) { _textComponents = GetComponentsInChildren<TextMeshProUGUI>(true); }
                foreach (TextMeshProUGUI text in _textComponents)
                {
                    if (theme.Font)
                    {
                        text.font = theme.Font;
                    }
                }

                foreach (Image panel in _panels)
                {
                    panel.material = theme.PanelMaterial;
                    panel.color = theme.PanelColor;
                }
            }
        }

        private void Update()
        {
            if (!IsActive)
            {
                if (_keyConfig.ShowConsoleKey.IsPressed() || _keyConfig.ToggleConsoleVisibilityKey.IsPressed())
                {
                    Activate();
                }
            }
            else
            {
                FlushQueuedLogs();
                ProcessAsyncTasks();
                HandleAsyncJobCounter();

                if (_keyConfig.HideConsoleKey.IsPressed() || _keyConfig.ToggleConsoleVisibilityKey.IsPressed())
                {
                    Deactivate();
                    return;
                }

                if (QuantumConsoleProcessor.TableIsGenerating)
                {
                    _consoleInput.interactable = false;
                    string consoleText = $"{_logTraceBuilder}\n{GetTableGenerationText()}".Trim();
                    if (consoleText != _consoleLogText.text)
                    {
                        if (_showInitLogs)
                        {
                            OnStateChange?.Invoke();
                            _consoleLogText.text = consoleText;
                        }
                        if (_inputPlaceholderText) { _inputPlaceholderText.text = "Loading..."; }
                    }

                    return;
                }
                else if (IsBlockedByAsync)
                {
                    OnStateChange?.Invoke();
                    _consoleInput.interactable = false;
                    if (_inputPlaceholderText) { _inputPlaceholderText.text = "Executing async command..."; }
                }
                else if (!_consoleInput.interactable)
                {
                    OnStateChange?.Invoke();
                    _consoleInput.interactable = true;
                    if (_inputPlaceholderText) { _inputPlaceholderText.text = "Enter Command..."; }
                    OverrideConsoleInput(string.Empty);

                    if (_isGeneratingTable)
                    {
                        if (_showInitLogs)
                        {
                            AppendLogTrace(GetTableGenerationText());
                            _consoleLogText.text = _logTraceBuilder.ToString();
                        }
                        _isGeneratingTable = false;
                        ScrollConsoleToLatest();
                    }
                }

                _previousText = _currentText;
                _currentText = _consoleInput.text;
                if (_currentText != _previousText) { OnTextChange(); }

                if (Input.GetKeyDown(_keyConfig.SubmitCommandKey)) { InvokeCommand(); }
                if (_storeCommandHistory) { ProcessCommandHistory(); }

                ProcessAutocomplete();
            }
        }

        private void LateUpdate()
        {
            if (IsActive)
            {
                FlushToConsoleText();
            }
        }

        private string GetTableGenerationText()
        {
            string text = $"Q:\\>Quantum Console Processor is initialising";
            text += $"\nQ:\\>Table generation under progress";
            text += $"\nQ:\\>{QuantumConsoleProcessor.LoadedCommandCount} commands have been loaded";
            if (QuantumConsoleProcessor.TableIsGenerating) { text += "..."; }
            else { text += ColorExtensions.ColorText($"\nQ:\\>Quantum Console Processor ready", _theme ? _theme.SuccessColor : Color.white); }

            return text;
        }

        private void ProcessCommandHistory()
        {
            if (Input.GetKeyDown(_keyConfig.NextCommandKey) || Input.GetKeyDown(_keyConfig.PreviousCommandKey))
            {
                if (Input.GetKeyDown(_keyConfig.NextCommandKey)) { _selectedPreviousCommandIndex++; }
                else if (_selectedPreviousCommandIndex > 0) { _selectedPreviousCommandIndex--; }
                _selectedPreviousCommandIndex = Mathf.Clamp(_selectedPreviousCommandIndex, -1, _previousCommands.Count - 1);

                if (_selectedPreviousCommandIndex > -1)
                {
                    string command = _previousCommands[_previousCommands.Count - _selectedPreviousCommandIndex - 1];
                    OverrideConsoleInput(command);
                }
            }
        }

        private void GetCommandSuggestions()
        {
            _suggestedCommands.Clear();
            _suggestedCommands.AddRange(QuantumConsoleProcessor.GetCommandSuggestions(_currentText, _useFuzzySearch, _caseSensitiveSearch, true));
        }

        private void ProcessAutocomplete()
        {
            if ((_keyConfig.SuggestNextCommandKey.IsPressed() || _keyConfig.SuggestPreviousCommandKey.IsPressed()) && !string.IsNullOrWhiteSpace(_currentText))
            {
                if (_selectedSuggestionCommandIndex < 0)
                {
                    _selectedSuggestionCommandIndex = -1;
                    GetCommandSuggestions();
                }

                if (_suggestedCommands.Count > 0)
                {
                    if (_keyConfig.SuggestPreviousCommandKey.IsPressed()) { _selectedSuggestionCommandIndex--; }
                    else if (_keyConfig.SuggestNextCommandKey.IsPressed()) { _selectedSuggestionCommandIndex++; }

                    _selectedSuggestionCommandIndex += _suggestedCommands.Count;
                    _selectedSuggestionCommandIndex %= _suggestedCommands.Count;
                    SetCommandSuggestion(_suggestedCommands[_selectedSuggestionCommandIndex]);
                }
            }
        }

        private string FormatSuggestion(CommandData command, bool selected)
        {
            if (!_theme) { return command.CommandSignature; }
            else
            {
                Color nameColor = Color.white;
                Color signatureColor = _theme.SuggestionColor;
                if (selected)
                {
                    nameColor *= _theme.SelectedSuggestionColor;
                    signatureColor *= _theme.SelectedSuggestionColor;
                }

                string nameSignature = command.CommandName.ColorText(nameColor);
                string genericSignature = command.GenericSignature.ColorText(signatureColor);
                string paramSignature = command.ParamaterSignature.ColorText(signatureColor);
                return $"{nameSignature}{genericSignature} {paramSignature}";
            }
        }

        private void ProcessPopupDisplay()
        {
            if (string.IsNullOrWhiteSpace(_currentText)) { ClearPopup(); }
            else
            {
                if (_selectedSuggestionCommandIndex < 0) { GetCommandSuggestions(); }
                if (_suggestedCommands.Count == 0) { ClearPopup(); }
                else
                {
                    if (_suggestionPopupRect && _suggestionPopupText)
                    {
                        int displaySize = _suggestedCommands.Count;
                        if (_maxSuggestionDisplaySize > 0) { displaySize = Mathf.Min(displaySize, _maxSuggestionDisplaySize + 1); }

                        IEnumerable<string> suggestions = GetFormattedCommandSuggestions(displaySize);
                        if (_suggestionDisplayOrder == SortOrder.Ascending) { suggestions = suggestions.Reverse(); }
                        _suggestionPopupRect.gameObject.SetActive(true);
                        _suggestionPopupText.text = string.Join("\n", suggestions);
                    }
                }
            }
        }

        private IEnumerable<string> GetFormattedCommandSuggestions(int displaySize)
        {
            for (int i = 0; i < displaySize; i++)
            {
                if (_maxSuggestionDisplaySize > 0 && i >= _maxSuggestionDisplaySize)
                {
                    const string remainingSuggestion = "...";
                    if (_theme)
                    {
                        if (_selectedSuggestionCommandIndex >= _maxSuggestionDisplaySize)
                        {
                            yield return remainingSuggestion.ColorText(_theme.SelectedSuggestionColor);
                        }
                        else
                        {
                            yield return remainingSuggestion;
                        }
                    }
                }
                else
                {
                    bool selected = i == _selectedSuggestionCommandIndex;
                    yield return FormatSuggestion(_suggestedCommands[i], selected);
                }
            }
        }

        private void SetCommandSuggestion(CommandData command)
        {
            OverrideConsoleInput(command.CommandName);
            Color suggestionColor = _theme ? _theme.SuggestionColor : Color.gray;
            _consoleSuggestionText.text = $"{ColorExtensions.ColorText(command.CommandName, Color.clear)}{ColorExtensions.ColorText(command.GenericSignature, suggestionColor)} {ColorExtensions.ColorText(command.ParamaterSignature, suggestionColor)}";
        }

        private void OverrideConsoleInput(string newInput)
        {
            _currentText = newInput;
            _previousText = newInput;
            _consoleInput.text = newInput;
            _consoleInput.Select();
            _consoleInput.caretPosition = newInput.Length;
            _consoleInput.selectionAnchorPosition = newInput.Length;
            _consoleInput.MoveTextEnd(false);
            _consoleInput.ActivateInputField();
            OnTextChange();
        }

        private void OnTextChange()
        {
            if (_selectedPreviousCommandIndex >= 0 && _currentText.Trim() != _previousCommands[_previousCommands.Count - _selectedPreviousCommandIndex - 1]) { ClearHistoricalSuggestions(); }
            if (_selectedSuggestionCommandIndex >= 0 && _currentText.Trim() != _suggestedCommands[_selectedSuggestionCommandIndex].CommandName) { ClearSuggestions(); }

            if (_showPopupDisplay) { ProcessPopupDisplay(); }
        }

        private void ClearHistoricalSuggestions()
        {
            _selectedPreviousCommandIndex = -1;
        }

        private void ClearSuggestions()
        {
            _selectedSuggestionCommandIndex = -1;
            _consoleSuggestionText.text = string.Empty;
        }

        private void ClearPopup()
        {
            if (_suggestionPopupRect) { _suggestionPopupRect.gameObject.SetActive(false); }
            if (_suggestionPopupText) { _suggestionPopupText.text = string.Empty; }
        }

        /// <summary>
        /// Invokes the command currently inputted into the Quantum Console.
        /// </summary>
        public void InvokeCommand()
        {
            if (!string.IsNullOrWhiteSpace(_consoleInput.text))
            {
                string command = _consoleInput.text.Trim();
                InvokeCommand(command);
                OverrideConsoleInput(string.Empty);
                StoreCommand(command);
            }
        }

        /// <summary>
        /// Invokes the given command.
        /// </summary>
        /// <param name="command">The command to invoke.</param>
        /// <returns>The return value, if any, of the invoked command.</returns>
        public object InvokeCommand(string command)
        {
            object commandResult = null;
            if (!string.IsNullOrWhiteSpace(command))
            {
                string commandLog = $"> {command}";
                if (_theme) { commandLog = commandLog.ColorText(_theme.CommandLogColor); }
                LogToConsole(commandLog);

                string logTrace = string.Empty;
                try
                {
                    commandResult = QuantumConsoleProcessor.InvokeCommand(command);

                    if (commandResult is Task task) { _currentTasks.Add(task); }
                    else { logTrace = _serializer.SerializeFormatted(commandResult, _theme); }
                }
                catch (System.Reflection.TargetInvocationException e) { logTrace = GetInvocationErrorMessage(e.InnerException); }
                catch (Exception e) { logTrace = GetErrorMessage(e); }

                LogToConsole(logTrace);
                OnInvoke?.Invoke(command);

                if (_autoScroll == AutoScrollOptions.OnInvoke) { ScrollConsoleToLatest(); }
                if (_closeOnSubmit) { Deactivate(); }
            }
            else { OverrideConsoleInput(string.Empty); }

            return commandResult;
        }

        [Command("qc-script-extern", "Executes an external source of QC script file, where each line is a seperate QC command.", MonoTargetType.Registry, Platform.AllPlatforms ^ Platform.WebGLPlayer)]
        public async Task InvokeExternalCommandsAsync(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string command = await reader.ReadLineAsync();
                    if (InvokeCommand(command) is Task ret)
                    {
                        await ret;
                        ProcessAsyncTasks();
                    }
                }
            }
        }

        private string GetErrorMessage(Exception e)
        {
            if (_verboseErrors) { return $"Quantum Processor Error ({e.GetType()}): {e.Message}\n{e.StackTrace}"; }
            else { return $"Quantum Processor Error: {e.Message}"; }
        }

        private string GetInvocationErrorMessage(Exception e)
        {
            if (_verboseErrors) { return $"Error ({e.GetType()}): {e.Message}\n{e.StackTrace}"; }
            else { return $"Error: {e.Message}"; }
        }

        /// <summary>Thread safe API to format and log text to the Quantum Console.</summary>
        /// <param name="logText">Text to be logged.</param>
        /// <param name="logType">The type of the log to be logged.</param>
        public void LogToConsoleAsync(string logText, LogType logType = LogType.Log)
        {
            Log log = new Log
            {
                LogText = logText,
                LogType = logType
            };

            LogToConsoleAsync(log);
        }

        /// <summary>Thread safe API to format and log text to the Quantum Console.</summary>
        /// <param name="log">Log to be logged.</param>
        public void LogToConsoleAsync(Log log)
        {
            if (!string.IsNullOrWhiteSpace(log.LogText))
            {
                log.LogText = FormatTrace(log.LogText);
                OnLog?.Invoke(log.LogText);
                _queuedLogs.Enqueue(log);
            }
        }

        private void FlushQueuedLogs()
        {
            bool scroll = false;
            bool open = false;

            while (!_queuedLogs.IsEmpty)
            {
                if (_queuedLogs.TryDequeue(out Log log))
                {
                    AppendLogTrace(log.LogText);

                    LoggingThreshold severity = log.LogType.ToLoggingThreshold();
                    scroll &= _autoScroll == AutoScrollOptions.Always;
                    open |= severity <= _openOnLogLevel;
                }
            }

            if (scroll) { ScrollConsoleToLatest(); }
            if (open) { Activate(); }
        }

        private void ProcessAsyncTasks()
        {
            for (int i = _currentTasks.Count - 1; i >= 0; i--)
            {
                if (_currentTasks[i].IsCompleted)
                {
                    if (_currentTasks[i].IsFaulted)
                    {
                        foreach (Exception e in _currentTasks[i].Exception.InnerExceptions)
                        {
                            string error = GetInvocationErrorMessage(e);
                            LogToConsole(error);
                        }
                    }
                    else
                    {
                        Type taskType = _currentTasks[i].GetType();
                        if (taskType.IsGenericTypeOf(typeof(Task<>)) && !_voidTaskType.IsAssignableFrom(taskType))
                        {
                            System.Reflection.PropertyInfo resultProperty = _currentTasks[i].GetType().GetProperty("Result");
                            object result = resultProperty.GetValue(_currentTasks[i]);
                            string log = _serializer.SerializeFormatted(result, _theme);
                            LogToConsole(log);
                        }
                    }

                    _currentTasks.RemoveAt(i);
                }
            }
        }

        private void HandleAsyncJobCounter()
        {
            if (_showCurrentJobs)
            {
                if (_jobCounterRect && _jobCounterText)
                {
                    if (_currentTasks.Count == 0) { _jobCounterRect.gameObject.SetActive(false); }
                    else
                    {
                        _jobCounterRect.gameObject.SetActive(true);
                        _jobCounterText.text = $"{_currentTasks.Count} job{(_currentTasks.Count == 1 ? "" : "s")} in progress";
                    }
                }
            }
        }

        /// <summary>
        /// Formats and logs text to the Quantum Console.
        /// </summary>
        /// <param name="logText">Text to be logged.</param>
        public void LogToConsole(string logText)
        {
            bool logExists = !string.IsNullOrWhiteSpace(logText);

            FlushQueuedLogs();
            if (logExists)
            {
                AppendLogTrace(FormatTrace(logText));
                OnLog?.Invoke(logText);

                if (_autoScroll == AutoScrollOptions.Always)
                {
                    ScrollConsoleToLatest();
                }
            }
        }

        private void FlushToConsoleText()
        {
            if (_consoleRequiresFlush)
            {
                _consoleRequiresFlush = false;
                _consoleLogText.text = _logTraceBuilder.ToString();
            }
        }

        private void AppendLogTrace(string logText)
        {
            _consoleRequiresFlush = true;
            _consoleLogs.Add(logText);

            bool initialLog = _logTraceBuilder.Length == 0;
            int logLength = _logTraceBuilder.Length + logText.Length;
            if (!initialLog) { logLength++; }

            if (_maxStoredLogs > 0)
            {
                while (_consoleLogs.Count > _maxStoredLogs)
                {
                    int junkLength = Mathf.Min(_consoleLogs[0].Length + 1, _logTraceBuilder.Length);
                    logLength -= junkLength;
                    _logTraceBuilder.Remove(0, junkLength);

                    _consoleLogs.RemoveAt(0);
                }
            }

            int capacity = _logTraceBuilder.Capacity;
            while (capacity < logLength)
            {
                capacity *= 2;
            }
            _logTraceBuilder.EnsureCapacity(capacity);

            if (!initialLog) { _logTraceBuilder.Append('\n'); }
            _logTraceBuilder.Append(logText);
        }

        private void ScrollConsoleToLatest()
        {
            if (_scrollRect)
            {
                _scrollRect.verticalNormalizedPosition = 0;
            }
        }

        private void StoreCommand(string command)
        {
            if (_storeCommandHistory)
            {
                if (!_storeDuplicateCommands) { _previousCommands.Remove(command); }
                if (_storeAdjacentDuplicateCommands || _previousCommands.Count == 0 || _previousCommands[_previousCommands.Count - 1] != command) { _previousCommands.Add(command); }
                if (_commandHistorySize > 0 && _previousCommands.Count > _commandHistorySize) { _previousCommands.RemoveAt(0); }
            }
        }

        private string FormatTrace(string logTrace)
        {
            if (_theme)
            {
                if (logTrace.ContainsCaseInsensitive("error")) { return ColorExtensions.ColorText(logTrace, _theme.ErrorColor); }
                else if (logTrace.ContainsCaseInsensitive("warning")) { return ColorExtensions.ColorText(logTrace, _theme.WarningColor); }
                else if (logTrace.ContainsCaseInsensitive("success")) { return ColorExtensions.ColorText(logTrace, _theme.SuccessColor); }
                else { return logTrace; }
            }
            else { return logTrace; }
        }

        /// <summary>
        /// Clears the Quantum Console.
        /// </summary>
        [Command("clear", "Clears the Quantum Console", MonoTargetType.Registry)]
        public void ClearConsole()
        {
            _consoleLogs.Clear();
            _logTraceBuilder.Length = 0;
            _consoleLogText.text = string.Empty;
            _consoleLogText.SetLayoutDirty();
            ClearBuffers();
            OnClear?.Invoke();
        }

        private void ClearBuffers()
        {
            ClearHistoricalSuggestions();
            ClearSuggestions();
            ClearPopup();
        }

        private void OnEnable()
        {
            QuantumRegistry.RegisterObject(this);
            Application.logMessageReceivedThreaded += DebugIntercept;

            if (IsSupportedState())
            {
                if (_singletonMode)
                {
                    if (Instance == null)
                    {
                        Instance = this;
                        DontDestroyOnLoad(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }

                if (_activateOnStartup) { Activate(); }
                else
                {
                    if (_initialiseOnStartup) { Initialise(); }
                    Deactivate();
                }
            }
            else { DisableQC(); }
        }

        private bool IsSupportedState()
        {
            SupportedState currentState = SupportedState.Always;
#if DEVELOPMENT_BUILD
            currentState = SupportedState.Development;
#elif UNITY_EDITOR
            currentState = SupportedState.Editor;
#endif
#if QC_DISABLED
            currentState = SupportedState.Never;
#endif
            return _supportedState <= currentState;
        }

        private void OnDisable()
        {
            QuantumRegistry.DeregisterObject(this);
            Application.logMessageReceivedThreaded -= DebugIntercept;

            Deactivate();
        }

        private void DisableQC()
        {
            Deactivate();
            enabled = false;
        }

        private void Initialise()
        {
            if (!QuantumConsoleProcessor.TableGenerated)
            {
                QuantumConsoleProcessor.GenerateCommandTable(true);
                _consoleInput.interactable = false;
                _isGeneratingTable = true;
            }

            _consoleLogText.richText = true;
            _consoleSuggestionText.richText = true;

            ApplyTheme(_theme);
            if (!_keyConfig) { _keyConfig = ScriptableObject.CreateInstance<QuantumKeyConfig>(); }
        }

        /// <summary>
        /// Toggles the Quantum Console.
        /// </summary>
        public void Toggle()
        {
            if (IsActive) { Deactivate(); }
            else { Activate(); }
        }

        /// <summary>
        /// Activates the Quantum Console.
        /// </summary>
        public void Activate()
        {
            Initialise();
            IsActive = true;
            _containerRect.gameObject.SetActive(true);
            OverrideConsoleInput(string.Empty);

            OnActivate?.Invoke();
        }

        /// <summary>
        /// Deactivates the Quantum Console.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            _containerRect.gameObject.SetActive(false);

            OnDeactivate?.Invoke();
        }

        private void DebugIntercept(string condition, string stackTrace, LogType type)
        {
            if (_interceptDebugLogger && (IsActive || _interceptWhilstInactive))
            {
                if (_prependTimestamps)
                {
                    DateTime now = DateTime.Now;
                    condition = $"[{now.Hour}:{now.Minute}:{now.Second}] {condition}";
                }

                if (_theme)
                {
                    switch (type)
                    {
                        case LogType.Log:
                            {
                                if (_verboseLogging >= LoggingThreshold.Always) { condition += $"\n{stackTrace}"; }
                                break;
                            }
                        case LogType.Warning:
                            {
                                if (_verboseLogging >= LoggingThreshold.Warning) { condition += $"\n{stackTrace}"; }
                                condition = ColorExtensions.ColorText(condition, _theme.WarningColor);
                                break;
                            }
                        case LogType.Error:
                            {
                                if (_verboseLogging >= LoggingThreshold.Error) { condition += $"\n{stackTrace}"; }
                                condition = ColorExtensions.ColorText(condition, _theme.ErrorColor);
                                break;
                            }
                        case LogType.Assert:
                            {
                                if (_verboseLogging >= LoggingThreshold.Error) { condition += $"\n{stackTrace}"; }
                                condition = ColorExtensions.ColorText(condition, _theme.ErrorColor);
                                break;
                            }
                        case LogType.Exception:
                            {
                                if (_verboseLogging >= LoggingThreshold.Exception) { condition += $"\n{stackTrace}"; }
                                condition = ColorExtensions.ColorText(condition, _theme.ErrorColor);
                                break;
                            }
                    }
                }

                LogToConsoleAsync(condition, type);
            }
        }
    }
}
