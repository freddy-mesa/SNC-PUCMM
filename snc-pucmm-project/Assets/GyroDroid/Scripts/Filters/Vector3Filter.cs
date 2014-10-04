using UnityEngine;

public class Vector3Filter : Filter<Vector3>
{
    public override void UpdateFunc(Vector3 input)
    {
        Holder = Vector3.Lerp(Holder, input, Time.deltaTime * Hardness);
    }
    public Vector3Filter(float hardness) : base(hardness)
    {
    }
}