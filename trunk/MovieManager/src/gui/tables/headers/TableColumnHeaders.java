/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui.tables.headers;

import java.util.ArrayList;
import java.util.HashMap;

/**
 *
 * @author alexander
 */
public class TableColumnHeaders {
    public static TableColumnHeaders instance = new TableColumnHeaders();
    
    private TableColumnHeaders(){
	addColumn(TableColumnHeaders.ColumnsEnum.Name.toString(),Boolean.TRUE);
	addColumn(TableColumnHeaders.ColumnsEnum.Path.toString(),Boolean.TRUE);
	addColumn(TableColumnHeaders.ColumnsEnum.VideoType.toString(),Boolean.TRUE);
	addColumn(TableColumnHeaders.ColumnsEnum.Rating.toString(),Boolean.TRUE);
    }
    
    private ArrayList<String> columns = new ArrayList<String>();
    private ArrayList<Boolean> visibility = new ArrayList<Boolean>();
    public static enum ColumnsEnum { Name , Path, VideoType, Rating}
    
    public static TableColumnHeaders getInstance(){
	return instance;
    }
    
    public ArrayList<String> getVisibleColumns(){
	ArrayList<String> retVal = new ArrayList<String>();
	for(int i = 0; i < columns.size() ; i++){
	    if(visibility.get(i)){
		retVal.add(columns.get(i));
	    }
	}
	return retVal;
    }
    
    public HashMap<String,Boolean> getColumns(){
	HashMap<String,Boolean> retVal = new HashMap<String,Boolean>();
	for(int i = 0; i < columns.size() ; i++){
		retVal.put(columns.get(i),visibility.get(i));
	}
	return retVal;
    }
    
    public ArrayList<String> getColumnsInOrder(){
	return columns;
    }
    
    public int getVisibleColumnsCount(){
	int retVal=0;
	for(int i = 0; i < columns.size() ; i++){
	    if(visibility.get(i)){
		retVal++;
	    }
	}
	return retVal;
    }
    
    public void switchColumns(int fromIndex, int toIndex) {
	if( !(isIndexOutOfBounds(toIndex) || isIndexOutOfBounds(fromIndex))){
	    //store from value
	    Boolean bHulp = visibility.get(fromIndex);
	    String sHulp = columns.get(fromIndex);
	    
	    //move toColumn to fromColumn
	    visibility.set(fromIndex, visibility.get(toIndex));
	    columns.set(fromIndex, columns.get(toIndex));
	    
	    //set toColumn
	    visibility.set(toIndex, bHulp);
	    columns.set(toIndex, sHulp);
	}
    }
    
    public void setColumnVisibility(String col, Boolean visibility){
	int index = columns.indexOf(col);
	if(!isIndexOutOfBounds(index)){
	    this.visibility.set(index, visibility);
	}
    }
    
    private boolean isIndexOutOfBounds(int index){
	return (index < 0 || index > columns.size());
    }
    
    private void addColumn(String name, Boolean visibility){
	columns.add(name);
	this.visibility.add(visibility);
    }
    
    private void insertColumn(int index, String name, Boolean visibility){
	columns.add(index, name);
	this.visibility.add(index,visibility);
    }
    
}
