using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetX
{
    public interface Particle
    {
        String getName();

        void setName(String name);

        Kind getKind();

        void setKind(Kind kind);

        String getConfigPath();

        void setConfigPath(String config);

        String getSpecificPath();

        void setSpecificPath(String spec);

        List<String> getVolumesPath();

        void setVolumesPath(List<String> volumes);

        ActOn getActOn();

        void setActOn(ActOn act);

        List<Volume> GetVolumeList();
    }

    public enum Kind
    {
        DecorKind, ChangeKind, TravelKind
    }

    public enum ActOn
    {
        Line, Syllable, Letter, WithoutSyllable, WithoutLetter
    }

    public abstract class GenericParticle : Particle
    {
        protected String name;
        protected Kind kind;
        protected String config;
        protected String specific;
        protected List<String> list_of_volumes;
        protected ActOn act;

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public Kind getKind()
        {
            return kind;
        }

        public void setKind(Kind kind)
        {
            this.kind = kind;
        }

        public String getConfigPath()
        {
            return config;
        }

        public void setConfigPath(String config)
        {
            this.config = config;
        }

        public String getSpecificPath()
        {
            return specific;
        }

        public void setSpecificPath(String specific)
        {
            this.specific = specific;
        }

        public List<String> getVolumesPath()
        {
            return list_of_volumes;
        }

        public void setVolumesPath(List<String> volumes)
        {
            list_of_volumes = volumes;
        }

        public ActOn getActOn()
        {
            return act;
        }

        public void setActOn(ActOn act)
        {
            this.act = act;
        }

        public List<Volume> GetVolumeList()
        {
            return new List<Volume>();
        }
    }

    public class ParamTypeParticle : GenericParticle
    {
        private TreeNode topNode = new TreeNode();
        private Dictionary<Volume, TreeNode> pointsNode_of_volume = new Dictionary<Volume, TreeNode>();
        private TreeNode volumesNode = new TreeNode("Volumes");
        private TreeNode accessParamsNode = new TreeNode("Set Parameters");
        private Dictionary<TreeNode, InsertPoint> points = new Dictionary<TreeNode, InsertPoint>();
        private Dictionary<TreeNode, Volume> volumes = new Dictionary<TreeNode, Volume>();
        private Dictionary<TreeNode, Event> events_of_volumes = new Dictionary<TreeNode, Event>();
        private int countPoints = 0;

        public ParamTypeParticle()
        {

        }

        public ParamTypeParticle(TreeNode top)
        {
            topNode = top;
            top.Nodes.Add(volumesNode);
            top.Nodes.Add(accessParamsNode);
        }

        public void InitTopNode(TreeNode top)
        {
            topNode = top;
            top.Nodes.Add(volumesNode);
            top.Nodes.Add(accessParamsNode);
        }

        public void AddVolumeWithEvents(Volume v, List<String> event_instants)
        {
            TreeNode volume_treenode = new TreeNode(v.Name);
            volumesNode.Nodes.Add(volume_treenode);
            volumes.Add(volume_treenode, v);

            foreach (String instant in event_instants)
            {
                Event ev = new Event();
                ev.Name = instant;
                TreeNode event_treenode = new TreeNode(instant);
                volume_treenode.Nodes.Add(event_treenode);
                events_of_volumes.Add(event_treenode, ev);
            }
        }

        public void AddVolume(Volume v)
        {
            TreeNode volume_treenode = new TreeNode(v.Name);
            volumesNode.Nodes.Add(volume_treenode);
            volumes.Add(volume_treenode, v);

            foreach (Event ev in v.Events.ToList())
            {
                TreeNode event_treenode = new TreeNode(ev.Name);
                volume_treenode.Nodes.Add(event_treenode);
                events_of_volumes.Add(event_treenode, ev);
            }

            TreeNode pointsNode = null;

            pointsNode = new TreeNode("Points");
            pointsNode_of_volume.Add(v, pointsNode);
            volume_treenode.Nodes.Add(pointsNode);

            countPoints++;
            TreeNode point_treenode = new TreeNode("Point " + countPoints);
            pointsNode.Nodes.Add(point_treenode);
            InsertPoint ip = new InsertPoint();
            v.GettInsertPoints().Add(ip);
            points.Add(point_treenode, ip);
        }

        public void ImportVolume(Volume v)
        {
            TreeNode volume_treenode = new TreeNode(v.Name);
            volumesNode.Nodes.Add(volume_treenode);
            volumes.Add(volume_treenode, v);

            foreach (Event ev in v.Events.ToList())
            {
                TreeNode event_treenode = new TreeNode(ev.Name);
                volume_treenode.Nodes.Add(event_treenode);
                events_of_volumes.Add(event_treenode, ev);
            }

            TreeNode pointsNode = null;

            pointsNode = new TreeNode("Points");
            pointsNode_of_volume.Add(v, pointsNode);
            volume_treenode.Nodes.Add(pointsNode);

            foreach(InsertPoint ip in v.GettInsertPoints()){
                countPoints++;
                TreeNode point_treenode = new TreeNode("Point " + countPoints);
                pointsNode.Nodes.Add(point_treenode);
                points.Add(point_treenode, ip);
            }
        }

        public void AddInsertPoint(Volume v)
        {
            TreeNode pointsNode = null;
            pointsNode_of_volume.TryGetValue(v, out pointsNode);

            countPoints++;
            TreeNode point_treenode = new TreeNode("Point " + countPoints);
            pointsNode.Nodes.Add(point_treenode);
            InsertPoint ip = new InsertPoint();
            v.GettInsertPoints().Add(ip);
            points.Add(point_treenode, ip);
        }

        public Dictionary<TreeNode, InsertPoint> GetPoints()
        {
            return points;
        }

        public Dictionary<TreeNode, Volume> GetVolumes()
        {
            return volumes;
        }

        public Dictionary<TreeNode, Event> GetEvents()
        {
            return events_of_volumes;
        }

        public TreeNode GetVolmuesNode()
        {
            return volumesNode;
        }

        //public TreeNode GetPointsNode()
        //{
        //    return pointsNode;
        //}

        public TreeNode GetPointsNode(Volume v)
        {
            TreeNode tn = new TreeNode();
            pointsNode_of_volume.TryGetValue(v, out tn);
            return tn;
        }

        public new List<Volume> GetVolumeList()
        {
            List<Volume> vols = new List<Volume>(volumes.Values);
            return vols;
        }
    }

    public class ScriptTypeParticle : GenericParticle
    {
        private TreeNode volumesNode = new TreeNode("Volumes");
        private TreeNode accessParamsNode = new TreeNode("Set Parameters");
        private Dictionary<TreeNode, Volume> volumes = new Dictionary<TreeNode, Volume>();
        private Dictionary<TreeNode, Event> events_of_volumes = new Dictionary<TreeNode, Event>();

        public ScriptTypeParticle()
        {

        }

        public ScriptTypeParticle(TreeNode top)
        {
            top.Nodes.Add(volumesNode);
            top.Nodes.Add(accessParamsNode);
        }

        public void InitTopNode(TreeNode top)
        {
            top.Nodes.Add(volumesNode);
            top.Nodes.Add(accessParamsNode);
        }

        public new List<Volume> GetVolumeList()
        {
            List<Volume> vols = new List<Volume>(volumes.Values);
            return vols;
        }
    }
}
