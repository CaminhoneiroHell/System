using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mathmatics : MonoBehaviour
{
    int a = 5;
    int b = 4;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(a + " + " + b + " = " + Add(a, b));
        Debug.Log(a + " - " + b + " = " + Subtract(a, b));
        Debug.Log(a + " * " + b + " = " + Multiply(a, b));
        Debug.Log(a + " / " + b + " = " + Divide(a, b));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int Add(int a, int b)
    {
        while (b != 0)
        {
            int c = a & b;
            a = a ^ b;
            b = c << 1;
        }
        return a;
    }

    
    int Subtract(int a, int b)
    {
        while (b != 0)
        {
            int borrow = (~a) & b;
            a = a ^ b;
            b = borrow << 1;
        }
        return a;
    }

    
    int Multiply(int n, int m)
    {
        int answer = 0;
        int count = 0;
        while (m != 0)
        {
            if (m % 2 == 1)
                answer += n << count;
            count++;
            m /= 2;
        }
        return answer;
    }
    
    int remainder, divisor;

    int Divide(int tempdividend, int tempdivisor)
    {
        int quotient = 1;

        if (tempdivisor == tempdividend)
        {
            remainder = 0;
            return 1;
        }
        else if (tempdividend < tempdivisor)
        {
            remainder = tempdividend;
            return 0;
        }

        do
        {

            tempdivisor = tempdivisor << 1;
            quotient = quotient << 1;

        } while (tempdivisor <= tempdividend);


        quotient = quotient +
             Divide(tempdividend - tempdivisor, divisor);

        return quotient;
    }


}
