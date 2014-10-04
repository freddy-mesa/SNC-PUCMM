#if !UNITY_ANDROID && !UNITY_EDITOR

using UnityEngine;
using System.Collections;

public class AndroidJavaObject {
	public T Call<T>(string method, params object[] parameters) {
		return default(T);
	}
	
	public void Call(string method, params object[] parameters) {}
	
	public AndroidJavaObject(params object[] parameters) {}
}

public class AndroidJavaClass {
	public AndroidJavaClass(string className) {}
	public T GetStatic<T>(string method, params object[] parameters) {
		return default(T);
	}
	public T CallStatic<T>(string method, params object[] parameters) {
		return default(T);
	}
	public void CallStatic(string method, params object[] parameters) {}
}

public class AndroidJNI {
	public static void AttachCurrentThread() {}
	
	public static System.IntPtr FindClass(params object[] parameters) {
		return System.IntPtr.Zero;
	}
	
	public static System.IntPtr GetStaticFieldID(params object[] parameters) {
		return System.IntPtr.Zero;
	}
	
	public static System.IntPtr GetStaticObjectField(params object[] parameters) {
		return System.IntPtr.Zero;
	}
	
	public static System.IntPtr GetMethodID(params object[] parameters) {
		return System.IntPtr.Zero;
	}
	
	public static System.IntPtr NewObject(params object[] parameters) {
		return System.IntPtr.Zero;
	}
}

public class jvalue {
	public object l;
}

#endif