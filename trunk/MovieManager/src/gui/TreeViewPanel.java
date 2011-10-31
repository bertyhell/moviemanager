/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gui;

import gui.tables.OverviewTable;
import gui.tables.VideoTableModel;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.util.Enumeration;
import javax.swing.JEditorPane;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTree;
import javax.swing.event.TreeSelectionEvent;
import javax.swing.event.TreeSelectionListener;
import javax.swing.tree.DefaultMutableTreeNode;
import javax.swing.tree.TreeNode;
import javax.swing.tree.TreePath;
import javax.swing.tree.TreeSelectionModel;
import logic.database.Video.VideoType;
import logic.database.VideoDatabase;

/**
 *
 * @author alexander
 */
public class TreeViewPanel extends JPanel implements TreeSelectionListener {

    private JTree tree;
    private JEditorPane htmlPane;

    public TreeViewPanel() {
	super(new GridLayout(1, 0));

	DefaultMutableTreeNode top = new DefaultMutableTreeNode("Video database");

	tree = new JTree(top);
	tree.getSelectionModel().setSelectionMode(TreeSelectionModel.SINGLE_TREE_SELECTION);
	tree.addTreeSelectionListener(this);
	
	//Categories node
	DefaultMutableTreeNode AllVideosNode = new DefaultMutableTreeNode("All Videos");
	top.add(AllVideosNode);
	
	//Categories node
	DefaultMutableTreeNode VideoTypeNode = new DefaultMutableTreeNode("Video Type");
	top.add(VideoTypeNode);

	//Add categories to categories node

	DefaultMutableTreeNode VideoCategory = new DefaultMutableTreeNode("Video");
	VideoTypeNode.add(VideoCategory);
	DefaultMutableTreeNode MovieCategory = new DefaultMutableTreeNode("Movie");
	VideoTypeNode.add(MovieCategory);
	DefaultMutableTreeNode EpisodeCategory = new DefaultMutableTreeNode("Episode");
	VideoTypeNode.add(EpisodeCategory);

	
	tree.setSelectionPath(new TreePath(AllVideosNode.getPath()));
	expandAll(tree, true);
	
	//Create the scroll pane and add the tree to it.
	JScrollPane treeScrollView = new JScrollPane(tree);


	Dimension minimumSize = new Dimension(100, 50);
	treeScrollView.setMinimumSize(minimumSize);

	//Add the split pane to this panel.
	add(treeScrollView);
    }

    @Override
    public void valueChanged(TreeSelectionEvent e) {
	String selectedNode = getSelectedNode(e);
	if(selectedNode.equals("All Videos")){
	    //set data in overview table
	    VideoTableModel.getInstance().updateData(VideoDatabase.getInstance().getVideos());
	}
	if(getParentNode(e).equals("Video Type")){
	    //set data in overview table
	    VideoTableModel.getInstance().updateData(VideoDatabase.getInstance().getVideosOfType(VideoType.valueOf(getSelectedNode(e))));
	}
    }
    
    private String getSelectedNode(TreeSelectionEvent e){
	return e.getPath().getLastPathComponent().toString();
    }
    
    private String getParentNode(TreeSelectionEvent e){
	return e.getPath().getParentPath().getLastPathComponent().toString();
    }

    public void expandAll(JTree tree, boolean expand) {
	TreeNode root = (TreeNode) tree.getModel().getRoot();

	// Traverse tree from root
	expandAll(tree, new TreePath(root), expand);
    }

    private void expandAll(JTree tree, TreePath parent, boolean expand) {
	// Traverse children
	TreeNode node = (TreeNode) parent.getLastPathComponent();
	if (node.getChildCount() >= 0) {
	    for (Enumeration e = node.children(); e.hasMoreElements();) {
		TreeNode n = (TreeNode) e.nextElement();
		TreePath path = parent.pathByAddingChild(n);
		expandAll(tree, path, expand);
	    }
	}

	// Expansion or collapse must be done bottom-up
	if (expand) {
	    tree.expandPath(parent);
	} else {
	    tree.collapsePath(parent);
	}
    }
}
