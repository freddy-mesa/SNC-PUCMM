#pragma strict

public var DatabaseName : String = "sncpucmm.s3db";
public var TableName : String = "CoordenadaLocalizacion";
var db : AccessDB;
 
function Start() {
    db = new AccessDB();
    db.OpenDB(DatabaseName);
}
 
var DatabaseEntryStringWidth = 100;
var scrollPosition : Vector2;
var databaseData : ArrayList = new ArrayList();
 
function OnGUI() {
    GUI.Box(Rect (25,25,Screen.width - 50, Screen.height - 50),""); 
    GUILayout.BeginArea(Rect(50, 50, Screen.width - 100, Screen.height - 100));
    
        GUILayout.BeginHorizontal();
        GUILayout.Label("Table Name: ");
            TableName = GUILayout.TextField(TableName, GUILayout.Width (160));
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
            if (GUILayout.Button ("Select All Data")) 
                databaseData = ReadFullTable();
            if (GUILayout.Button("Clear"))
                databaseData.Clear();
        GUILayout.EndHorizontal();
 
        GUILayout.Label("Table Content");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(450));
            for (var line : ArrayList in databaseData) {
                GUILayout.BeginHorizontal();
                for (var s in line) {
                    GUILayout.Label(s.ToString(), GUILayout.Width(DatabaseEntryStringWidth));
                }
                GUILayout.EndHorizontal();
            }
        GUILayout.EndScrollView();
    GUILayout.EndArea();
}
 
function InsertRow(firstName:String, lastName:String) {
    var values = new Array(("'"+firstName+"'"),("'"+lastName+"'"));
    db.InsertInto(TableName, values);
}
 
function ReadFullTable() {
    return db.ReadFullTable(TableName);
}
 
function DeleteTableContents() {
    db.DeleteTableContents(TableName);
}