using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* enum is the return and 'vegetabletype' is the type?
 * Is Vegetabletype a class within Item class?
 is enum the type or 'vegetabletype'
public VegetableType typeOfVeggie; -- this line is just to declare? is typeOfVeggie a variable or object?
 */
public class Item : MonoBehaviour
{
    public enum VegetableType
    {
        BEET,
        CARROT,
        RADISH
    }
    public VegetableType typeOfVeggie;
    // Start is called before the first frame update
}