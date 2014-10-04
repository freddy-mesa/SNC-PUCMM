using UnityEngine;

public class AngleFilter : FloatFilter
{
    public override void UpdateFunc(float input)
    {
        Holder = Mathf.LerpAngle(Holder, input, Time.deltaTime * Hardness);
    }
    public AngleFilter(float hardness) : base(hardness)
    {
    }
}