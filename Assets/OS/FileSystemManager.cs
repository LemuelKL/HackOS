using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystem
{
    public class FileNode
    {
        public string hash; // not really hash but more like tag/signature for internal use
        public bool isDirectory;
        public string name;
        public FileNode parent;
        public List<FileNode> children = null;
        public FileNode(bool folder = false, string name = "New File")
        {
            isDirectory = folder;
            this.name = name;
            if (folder) children = new List<FileNode>();
            parent = null;
        }
        public bool Add(FileNode fn)
        {
            if (fn == null) return false;
            fn.parent = this;
            bool nameCrash = GetChild(fn.name) != null;
            if (nameCrash) return false;
            children.Add(fn);
            return true;
        }
        public List<string> GetChildrenNames()
        {
            List<string> names = new List<string>();
            foreach (FileNode child in children)
            {
                names.Add(child.name);
            }
            return names;
        }
        public FileNode GetChild(string name)
        {
            foreach (FileNode child in children)
            {
                if (child.name == name)
                {
                    return child;
                }
            }
            return null;
        }
        // Query all file entries under this node if there's any
        public List<string> Query(string context = "")
        {
            List<string> entries = new List<string>();
            if (!isDirectory)
            {
                entries.Add(context + name);
                return entries;
            }
            entries.Add(context + name + "/");
            foreach (FileNode child in children)
            {
                List<string> childEntries = new List<string>();
                childEntries = child.Query(context + name + "/");
                foreach (string cE in childEntries)
                {
                    entries.Add(cE);
                }
            }
            return entries;
        }
        public bool NewFile(string name)
        {
            return this.Add(new FileNode(false, name));
        }
        public bool NewFolder(string name)
        {
            return this.Add(new FileNode(true, name));
        }
        public string GetPath()
        {
            List<string> pathComponents = new List<string>();
            pathComponents.Add(name);
            FileNode curParentNode = parent;
            while (curParentNode != null)
            {
                pathComponents.Add(curParentNode.name);
                curParentNode = curParentNode.parent;
            }
            pathComponents.Reverse();
            string ret = "";
            foreach (string pc in pathComponents)
            {
                ret += pc + "/";
            }
            ret = ret.Substring(0, ret.Length - 1); // remove last '/'
            return ret;
        }
    }

    public FileNode files = new FileNode(true, "");

    ///

    public List<string> GetAllFSEntries()
    {
        return files.Query();
    }
    public bool NewFile(string name, string location)
    {
        return this.Add(location, false, name);
    }
    public bool NewFolder(string name, string location)
    {
        return this.Add(location, true, name);
    }
    public FileNode GetFileNode(string fullPath)
    {
        string[] entryNames = fullPath.Split('/');
        FileNode curNode = files;
        for (int i = 1; i < entryNames.Length; i++)
        {
            FileNode childNode = curNode.GetChild(entryNames[i]);
            if (childNode == null)
                return null;
            curNode = childNode;
        }
        return curNode;
    }
    // damn im smart
    public bool Add(string parentPath, bool folder = false, string name = "New File")
    {
        FileNode parentFN = GetFileNode(parentPath);
        if (parentFN == null) return false;
        FileNode newFN = new FileNode(folder, name);
        return parentFN.Add(newFN);
    }
    public FileNode GetUserHome(string user)
    {
        return GetFileNode("/home/" + user);
    }
}

public class FileSystemManager : MonoBehaviour
{
    public FileSystem localhost;
    public Dictionary<string, FileSystem> remoteMachines; // ip, FileSystem
    void Awake()
    {
        localhost = new FileSystem();
        localhost.NewFolder("home", "");
        localhost.NewFolder("root", "/home");
        localhost.NewFolder("downloads", "/home/root");
        localhost.NewFolder("wordlists", "/home/root");

        localhost.NewFile("rockyou.txt", "/home/root/wordlists");

        remoteMachines = new Dictionary<string, FileSystem>();
    }

    public void AddFileSystem(string ip, FileSystem fs)
    {
        if (ip == "localhost" || ip == "127.0.0.1" || ip == "hack0s")
            return;
        if (remoteMachines.ContainsKey(ip))
            remoteMachines.Remove(ip);
        remoteMachines.Add(ip, fs);
    }

    public void RemoveFileSystem(string ip)
    {
        remoteMachines.Remove(ip);
    }

    private FileSystem GetFileSystemOfMachine(string ip)
    {
        if (ip == "localhost" || ip == "127.0.0.1" || ip == "hack0s")
            return localhost;
        return remoteMachines[ip];
    }

    public FileSystem.FileNode GetFileNode(string ip, string fullPath)
    {
        return GetFileSystemOfMachine(ip).GetFileNode(fullPath);
    }

    public FileSystem.FileNode GetRootNode(string ip)
    {
        return GetFileSystemOfMachine(ip).files; // actually this is fucking dangerous
    }
}
