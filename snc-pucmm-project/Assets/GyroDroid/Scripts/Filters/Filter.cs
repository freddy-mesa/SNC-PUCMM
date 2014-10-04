// these are really simple Lerp filters for several datatypes
// you declare one filter per variable you want to get filtered
// and feed each filter with its variable each frame
// -- the filter returns the filtered value

// declaration:
// 		FloatFilter k = new FloatFilter(1);
// ...
// usage:
// 		k.Update(newValue); // returns the filtered value
// and then
// 		k.Value				// returns the filtered value

public abstract class Filter<T>
{
    public T Holder;
    public float Hardness {get;set;}
	
    public abstract void UpdateFunc(T input);
	
    public T Update(T input)
    {
        UpdateFunc(input);
        return Holder;
    }
	
    public T Value
    {
        get
        {
            return Holder;
        }
    }
	
    public Filter(float hardness)
    {
        Holder = default(T);
        Hardness = hardness;
    }
}