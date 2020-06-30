using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="Inventory/Item")]

public class Item : ScriptableObject
{

    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public discriptor Discriptor;
    public attribute Attribute;


    public virtual void Use()
    {

    }
    
    public discriptor getDiscriptor()
    {
        return Discriptor;
    }

    public attribute getAttribute()
    {
        return Attribute;
    }

}
