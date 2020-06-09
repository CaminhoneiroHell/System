using UnityEngine;

public class ClickCommand : ICommand
{
    private GameObject m_cube;
    private Color m_color;
    private Color m_Previouscolor;

    public ClickCommand(GameObject cube, Color color)
    {
        this.m_cube = cube;
        this.m_color = color;
    }

    public void Execute()
    {
        m_Previouscolor = m_cube.GetComponent<MeshRenderer>().material.color;
        m_cube.GetComponent<MeshRenderer>().material.color = m_color;
    }

    public void Undue()
    {
        m_cube.GetComponent<MeshRenderer>().material.color = m_Previouscolor;
    }
}
