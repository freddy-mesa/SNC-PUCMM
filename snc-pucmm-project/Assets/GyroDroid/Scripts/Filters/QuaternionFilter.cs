using UnityEngine;

public class QuaternionFilter : Filter<Quaternion>
{
    public override void UpdateFunc(Quaternion input)
    {
        Holder = Quaternion.Lerp(Holder, input, Time.deltaTime * Hardness);
    }
    public QuaternionFilter(float hardness) : base(hardness)
    {
    }
}