package com.sncpucmm.app;
import com.unity3d.player.UnityPlayerActivity;
import android.content.Context;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.util.Log;

public class GPSPlugin extends UnityPlayerActivity {
    private static final String TAG = "GPS_Unity";
    
    /** Stores the current location */
    public static Location currentLocation;
    public static LocationManager myLocationManager;
    
    /** Listeners for the gps and network location */
    static LocationListener networkLocationListener;
    static LocationListener gpsLocationListener;
    
    public static void startLocationListeners() {
        /**
         * Gps location listener.
         */
        gpsLocationListener = new LocationListener() {
            @Override
            public void onLocationChanged(Location location) {
                currentLocation = location;
                Log.i(TAG, "Getting Location over GPS " + currentLocation.toString());
            }
            public void onProviderDisabled(String provider) {
            }
            public void onProviderEnabled(String provider) {
            }
            public void onStatusChanged(String provider, int status,
                    Bundle extras) {
            }
        };
        
        /**
         * Network location listener.
         */
        networkLocationListener = new LocationListener() {
            @Override
            public void onLocationChanged(Location location) {
                currentLocation = location;
                Log.i(TAG,
                        "Getting Location over GPS" + currentLocation.toString());
            }
            public void onProviderDisabled(String provider) {
            }
            public void onProviderEnabled(String provider) {
            }
            public void onStatusChanged(String provider, int status,
                    Bundle extras) {
            }
        };
        
        myLocationManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER,0, 0,
                networkLocationListener);
        myLocationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0,
                gpsLocationListener);
    }
    
    @Override
    protected void onCreate(Bundle myBundle) {
        super.onCreate(myBundle);
        myLocationManager = (LocationManager) getSystemService(Context.LOCATION_SERVICE);
        startLocationListeners();
    }
    
    @Override
    protected void onResume() {
        super.onResume();
        startLocationListeners();
    }
    
    @Override
    protected void onPause(){
        myLocationManager.removeUpdates(networkLocationListener);
        myLocationManager.removeUpdates(gpsLocationListener);
        super.onPause();
    }
    
    @Override
    protected void onStop() {
        myLocationManager.removeUpdates(networkLocationListener);
        myLocationManager.removeUpdates(gpsLocationListener);
        super.onStop();
    }
    
    static public float GetLatitude() {
        return currentLocation != null  ? (float)currentLocation.getLatitude() : 0.0f;
    }
   
    static public float GetLongitude(){
        return currentLocation != null  ? (float)currentLocation.getLongitude() : 0.0f;
    }
    
    static public float GetAltitude() {
        return currentLocation != null  ? (float)currentLocation.getAltitude() : 0.0f;
    }
    
    static public float GetAccuracy()  {
        return currentLocation != null  ? (float)currentLocation.getAccuracy() : 0.0f;
    } 
}