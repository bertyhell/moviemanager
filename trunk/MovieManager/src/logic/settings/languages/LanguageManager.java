/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package logic.settings.languages;

import gui.MainWindow;
import java.io.FileInputStream;
import java.io.IOException;
import java.util.Properties;
import javax.swing.JOptionPane;
import logic.settings.ProgramSettings;

/**
 *
 * @author alexander
 */
public class LanguageManager {

    private static LanguageManager instance = new LanguageManager();
    private Properties prop = new Properties();

    private LanguageManager() {
	ProgramSettings settings = ProgramSettings.getInstance();
	String languageFilePath = settings.getConfigurationConstants().getProperty("language_file_path") + "/";
	try {
	    prop.load(new FileInputStream(languageFilePath + settings.get("Language") + ".properties"));
	} catch (Exception ex) {
	    try {
		prop.load(new FileInputStream(languageFilePath + "en_uk.properties"));
	    } catch (IOException ex1) {
		JOptionPane.showMessageDialog(MainWindow.getInstance(), "Could not open language file en_uk.properties\n" + ex.getMessage(), "ERROR", JOptionPane.ERROR_MESSAGE);
	    }
	}
    }
    
    public static LanguageManager getInstance(){
	return instance;
    }
    
    public String get(String key){
	return (String) prop.get(key);
    }
}
