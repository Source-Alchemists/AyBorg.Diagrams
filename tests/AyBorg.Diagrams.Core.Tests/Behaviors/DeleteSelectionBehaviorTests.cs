﻿using System;
using System.Threading.Tasks;
using AyBorg.Diagrams.Core.Behaviors;
using AyBorg.Diagrams.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace AyBorg.Diagrams.Core.Tests.Behaviors
{
    public class DeleteSelectionBehaviorTests
    {
        [Fact]
        public void Behavior_ShouldNotRun_WhenItsRemoved()
        {
            // Arrange
            var diagram = new Diagram();
            diagram.UnregisterBehavior<DeleteSelectionBehavior>();
            diagram.Nodes.Add(new NodeModel
            {
                Selected = true
            });

            // Act
            diagram.OnKeyDown(new KeyboardEventArgs
            {
                AltKey = false,
                CtrlKey = false,
                ShiftKey = false,
                Code = "Delete",
            });

            // Assert
            diagram.Nodes.Count.Should().Be(1);
        }

        [Fact]
        public void Behavior_ShouldTakeIntoAccountDeleteKeyOption()
        {
            // Arrange
            var diagram = new Diagram();
            diagram.Options.DeleteKey = "Test";
            diagram.Nodes.Add(new NodeModel
            {
                Selected = true
            });

            // Act
            diagram.OnKeyDown(new KeyboardEventArgs
            {
                AltKey = false,
                CtrlKey = false,
                ShiftKey = false,
                Code = "Test",
            });

            // Assert
            diagram.Nodes.Count.Should().Be(0);
        }

        [Fact]
        public void Behavior_ShouldNotDeleteModel_WhenItsLocked()
        {
            // Arrange
            var diagram = new Diagram();
            diagram.Nodes.Add(new NodeModel
            {
                Selected = true,
                Locked = true
            });

            // Act
            diagram.OnKeyDown(new KeyboardEventArgs
            {
                AltKey = false,
                CtrlKey = false,
                ShiftKey = false,
                Code = "Delete",
            });

            // Assert
            diagram.Nodes.Count.Should().Be(1);
        }

        [Fact]
        public void Behavior_ShouldTakeIntoAccountGroupConstraint()
        {
            // Arrange
            var funcCalled = false;
            var diagram = new Diagram();
            diagram.Options.Constraints.ShouldDeleteGroup = _ =>
            {
                funcCalled = true;
                return Task.FromResult(false);
            };
            diagram.AddGroup(new GroupModel(Array.Empty<NodeModel>())
            {
                Selected = true
            });

            // Act
            diagram.OnKeyDown(new KeyboardEventArgs
            {
                AltKey = false,
                CtrlKey = false,
                ShiftKey = false,
                Code = "Delete",
            });

            // Assert
            funcCalled.Should().BeTrue();
            diagram.Groups.Count.Should().Be(1);
        }

        [Fact]
        public void Behavior_ShouldTakeIntoAccountNodeConstraint()
        {
            // Arrange
            var funcCalled = false;
            var diagram = new Diagram();
            diagram.Options.Constraints.ShouldDeleteNode = _ =>
            {
                funcCalled = true;
                return Task.FromResult(false);
            };
            diagram.Nodes.Add(new NodeModel
            {
                Selected = true
            });

            // Act
            diagram.OnKeyDown(new KeyboardEventArgs
            {
                AltKey = false,
                CtrlKey = false,
                ShiftKey = false,
                Code = "Delete",
            });

            // Assert
            funcCalled.Should().BeTrue();
            diagram.Nodes.Count.Should().Be(1);
        }

        [Fact]
        public void Behavior_ShouldTakeIntoAccountLinkConstraint()
        {
            // Arrange
            var funcCalled = false;
            var diagram = new Diagram();
            diagram.Options.Constraints.ShouldDeleteLink = _ =>
            {
                funcCalled = true;
                return Task.FromResult(false);
            };
            diagram.Nodes.Add(new NodeModel[]
            {
                new NodeModel(),
                new NodeModel()
            });
            diagram.Links.Add(new LinkModel(diagram.Nodes[0], diagram.Nodes[1])
            {
                Selected = true
            });

            // Act
            diagram.OnKeyDown(new KeyboardEventArgs
            {
                AltKey = false,
                CtrlKey = false,
                ShiftKey = false,
                Code = "Delete",
            });

            // Assert
            funcCalled.Should().BeTrue();
            diagram.Links.Count.Should().Be(1);
        }

        [Fact]
        public void Behavior_ShouldResultInSingleRefresh()
        {
            // Arrange
            var diagram = new Diagram();
            diagram.Nodes.Add(new NodeModel[]
            {
                new NodeModel { Selected = true },
                new NodeModel { Selected = true }
            });

            var refreshes = 0;
            diagram.Changed += () => refreshes++;

            // Act
            diagram.OnKeyDown(new KeyboardEventArgs
            {
                AltKey = false,
                CtrlKey = false,
                ShiftKey = false,
                Code = "Delete",
            });

            // Assert
            diagram.Nodes.Count.Should().Be(0);
            refreshes.Should().Be(1);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Behavior_ShouldNotDeleteModel_WhenCtrlAltOrShiftIsPressed(bool ctrl, bool shift, bool alt)
        {
            // Arrange
            var diagram = new Diagram();
            diagram.Nodes.Add(new NodeModel
            {
                Selected = true
            });

            // Act
            diagram.OnKeyDown(new KeyboardEventArgs
            {
                AltKey = alt,
                CtrlKey = ctrl,
                ShiftKey = shift,
                Code = "Delete",
            });

            // Assert
            diagram.Nodes.Count.Should().Be(1);
        }
    }
}
