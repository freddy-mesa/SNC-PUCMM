using UnityEngine;

public class FloatFilter : Filter<float>
{
    public override void UpdateFunc(float input)
    {
        Holder = Mathf.Lerp(Holder, input, Time.deltaTime * Hardness);
    }
    public FloatFilter(float hardness) : base(hardness)
    {
    }
}