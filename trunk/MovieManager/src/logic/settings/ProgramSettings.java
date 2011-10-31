package logic.settings;

import gui.MainWindow;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.StringWriter;
import java.util.HashMap;
import java.util.Properties;
import java.util.SortedSet;
import java.util.TreeSet;
import javax.swing.JOptionPane;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

/**
 *
 * @author Bert Verhelst <verhelstbert@gmail.com>
 */
public final class ProgramSettings {
    
    private static ProgramSettings programSettings ;
    
    public static ProgramSettings getInstance(){     
        if(programSettings == null){
            synchronized (ProgramSettings.class){
                if(programSettings == null){
		    try {
			programSettings = new ProgramSettings();
		    } catch (IOException ex) {
			JOptionPane.showMessageDialog(MainWindow.getInstance(), "Could not open settings file\n" + ex.getMessage(), "ERROR", JOptionPane.ERROR_MESSAGE); //TODO 005 make message use property file 
		    }
                    
                }
            }
        }
        return programSettings;
    }
        
    //user settings
    private HashMap<String, String> settings;
    private Properties prop = new Properties();

    private ProgramSettings() throws IOException {
        //load a properties file
        prop.load(new FileInputStream("settings/configurationConstants.properties"));
	read();
    }

    public Properties getConfigurationConstants() {
	return prop;
    }
    
    public String get(String key) {
        return settings.get(key);
    }

    public void set(String key, String value) {
        settings.put(key, value);
    }

    public void increase(String key) throws NumberFormatException {
        increase(key, 1);
    }

    public void decrease(String key) throws NumberFormatException {
        increase(key, -1);
    }

    public void increase(String key, int amount) throws NumberFormatException {
        settings.put(key, Integer.toString(Integer.parseInt(settings.get(key)) + amount));
    }

    public void read() {
        settings = new HashMap<String, String>();
//	System.out.println(prop.getProperty("settings_file"));
        File file = new File(prop.getProperty("settings_file"));
        try {
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            Document doc = db.parse(file);
            doc.getDocumentElement().normalize();

            NodeList nodeLst = doc.getElementsByTagName("Settings");
            NodeList settingNodes = nodeLst.item(0).getChildNodes();

            for (int i = 0; i < settingNodes.getLength(); i++) {
                Node node = settingNodes.item(i);
                if (!node.getNodeName().startsWith("#")) {
                    settings.put(node.getNodeName(), node.getTextContent());
                }
            }

//            print();
        } catch (Exception ex) {
            JOptionPane.showMessageDialog(MainWindow.getInstance(), "kan settings niet lezen, \ncontroleer of bestand settings.xml bestaat.\nDit is een ernstige fout, gelieve niet verder te gaan zonder dat dit opgelost wordt.\n" + ex.getMessage(), "Can't read settings", JOptionPane.ERROR_MESSAGE);
        }
    }

    public void write() {
        try {
            //create xml string
            DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
            DocumentBuilder db = dbf.newDocumentBuilder();
            Document doc = db.newDocument();

            //create the root element and add it to the document
            Element root = doc.createElement("Settings");
            doc.appendChild(root);

            //create child element, set content and add to root
            Element child = null;
            SortedSet<String> sortedset = new TreeSet<String>(settings.keySet());
            for (String key : sortedset) {
                try {
                    child = doc.createElement(key);
                    child.setTextContent(settings.get(key));
                    root.appendChild(child);
                } catch (Exception ex) {
                    System.out.println("catched exception:\n" + ex.getMessage());
                    ex.printStackTrace();
                }
            }

            //Output the XML

            //set up a transformer
            TransformerFactory transfac = TransformerFactory.newInstance();
            Transformer trans = transfac.newTransformer();
            trans.setOutputProperty(OutputKeys.OMIT_XML_DECLARATION, "yes");
            trans.setOutputProperty(OutputKeys.INDENT, "yes");

            //create string from xml tree
            StringWriter sw = new StringWriter();
            StreamResult result = new StreamResult(sw);
            DOMSource source = new DOMSource(doc);
            trans.transform(source, result);
            String xmlString = sw.toString();

            //print xml
            BufferedWriter output = new BufferedWriter(new FileWriter(new File(prop.getProperty("settings_file"))));
            output.write(xmlString);
            output.close();

        } catch (Exception ex) {
            JOptionPane.showMessageDialog(MainWindow.getInstance(), "kan settings niet wegschrijven, \ncontroleer of bestand settings.xml in gebruik is.\nDit is een ernstige fout, gelieve niet verder te gaan zonder dat dit opgelost wordt.\n" + ex.getMessage(), "Can't save settings", JOptionPane.ERROR_MESSAGE);
            ex.printStackTrace();
        }
    }

    public void print() {
        SortedSet<String> sortedset = new TreeSet<String>(settings.keySet());
        for (String key : sortedset) {
            System.out.println("|" + key + "|\t->\t|" + settings.get(key) + "|");
        }
    }
}
