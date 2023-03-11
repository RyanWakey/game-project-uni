using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity 
{
     Rigidbody2D rb2D { get; }
     Transform tr { get; }
}
