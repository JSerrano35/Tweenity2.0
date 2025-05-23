using UnityEngine.UIElements;
using UnityEngine;
using Models.Nodes;
using Controllers;

namespace Views.RightPanel
{
    public class EndView : TweenityNodeView
    {
        public EndView(EndNodeModel model, GraphController controller) : base(model, controller)
        {
            var title = new Label("End Node Details");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.whiteSpace = WhiteSpace.Normal;
            Add(title);

            var note = new Label("This is an End Node. No editable properties.");
            note.style.unityFontStyleAndWeight = FontStyle.Italic;
            note.style.marginTop = 10;
            note.style.whiteSpace = WhiteSpace.Normal;
            note.style.flexShrink = 0;
            Add(note);

            // Outgoing connections are not expected on End nodes, but show if present
            Add(new Label("Outgoing Connections")
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 10 }
            });

            foreach (var path in model.OutgoingPaths)
            {
                var label = new Label($"Connected to: {path.TargetNodeID}")
                {
                    style = { whiteSpace = WhiteSpace.Normal }
                };
                Add(label);
            }
        }
    }
}
