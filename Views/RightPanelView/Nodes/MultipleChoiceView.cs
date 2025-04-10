using UnityEngine.UIElements;
using UnityEngine;
using Models.Nodes;
using Controllers;
using System;

namespace Views.RightPanel
{
    public class MultipleChoiceView : TweenityNodeView
    {
        private ListView _choicesList;

        public MultipleChoiceView(MultipleChoiceNodeModel model, GraphController controller) : base(model, controller)
        {
            var title = new Label("Multiple Choice Node Details")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    whiteSpace = WhiteSpace.Normal
                }
            };
            Add(title);

            var questionLabel = new Label("Question");
            questionLabel.style.whiteSpace = WhiteSpace.Normal;
            Add(questionLabel);

            var questionField = new TextField { value = model.Question, multiline = true };
            questionField.RegisterValueChangedCallback(evt =>
            {
                model.Question = evt.newValue;
            });
            Add(questionField);

            var choicesLabel = new Label("Answers");
            choicesLabel.style.whiteSpace = WhiteSpace.Normal;
            Add(choicesLabel);

            var addChoiceButton = new Button(() =>
            {
                model.OutgoingPaths.Add(new PathData($"Choice {model.OutgoingPaths.Count + 1}"));
                _choicesList.Rebuild();
                controller.GraphView.RefreshNodeVisual(model.NodeID);
            })
            {
                text = "+ Add Answer"
            };
            Add(addChoiceButton);

            _choicesList = new ListView(model.OutgoingPaths, itemHeight: 50, makeItem: () =>
            {
                var container = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        justifyContent = Justify.SpaceBetween,
                        alignItems = Align.Center,
                        marginBottom = 5
                    }
                };

                var answerField = new TextField { style = { flexGrow = 1, marginRight = 5 } };
                var triggerButton = new Button(() => Debug.Log("[Trigger]")) { text = "Trigger" };
                var connectButton = new Button() { text = "Connect" };

                container.Add(answerField);
                container.Add(triggerButton);
                container.Add(connectButton);

                return container;
            },
            bindItem: (element, i) =>
            {
                var container = element as VisualElement;

                var answerField = container.ElementAt(0) as TextField;
                var connectButton = container.ElementAt(2) as Button;

                answerField.value = model.OutgoingPaths[i].Label;
                answerField.RegisterValueChangedCallback(evt =>
                {
                    model.OutgoingPaths[i].Label = evt.newValue;
                    controller.GraphView.RefreshNodeVisual(model.NodeID);
                });

                connectButton.clickable = new Clickable(() =>
                {
                    controller.StartConnectionFrom(model.NodeID, targetId =>
                    {
                        model.OutgoingPaths[i].TargetNodeID = targetId;
                        controller.GraphView.RenderConnections();
                    });
                });
            });

            Add(_choicesList);

            Add(new Label("Outgoing Paths")
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 10 }
            });

            foreach (var path in model.OutgoingPaths)
            {
                var label = new Label($"Path to: {path.TargetNodeID} (Label: {path.Label})")
                {
                    style = { whiteSpace = WhiteSpace.Normal }
                };
                Add(label);
            }
        }
    }
}
