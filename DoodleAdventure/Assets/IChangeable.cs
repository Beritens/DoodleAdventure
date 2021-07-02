using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChangeable
{
    void activate();
    void deactivate();
    void firstactivation();
}
