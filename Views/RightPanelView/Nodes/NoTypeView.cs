using UnityEngine.UIElements;
using UnityEngine;
using Models.Nodes;
using Controllers;

namespace Views.RightPanel
{
    public class NoTypeView : TweenityNodeView
    {
        public NoTypeView(NoTypeNodeModel model, GraphController controller) : base(model, controller)
        {
            var title = new Label("Generic Node Details");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.whiteSpace = WhiteSpace.Normal;
            Add(title);

            var label = new Label("This node has no specific properties.");
            label.style.unityFontStyleAndWeight = FontStyle.Italic;
            label.style.marginTop = 10;
            label.style.whiteSpace = WhiteSpace.Normal;
            label.style.flexShrink = 0;
            Add(label);

            if (model.OutgoingPaths.Count == 0)
            {
                var connectButton = new Button(() =>
                {
                    Debug.Log($"[NoTypeView] Connect clicked for NodeID: {model.NodeID}");
                    controller.StartConnectionFrom(model.NodeID, (targetNodeId) =>
                    {
                        model.OutgoingPaths.Add(new PathData("Next", "", targetNodeId));
                        controller.GraphView.RenderConnections();
                    });
                })
                {
                    text = "Connect"
                };
                connectButton.style.marginTop = 10;
                Add(connectButton);
            }
            else
            {
                Add(new Label("Outgoing Connection")
                {
                    style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 10 }
                });

                var path = model.OutgoingPaths[0];
                var connectedModel = controller.GetNode(path.TargetNodeID);
                var connectionLabel = new Label($"→ {connectedModel?.Title ?? "(Unknown)"}")
                {
                    style = { whiteSpace = WhiteSpace.Normal }
                };
                Add(connectionLabel);
            }
        }
    }
}
