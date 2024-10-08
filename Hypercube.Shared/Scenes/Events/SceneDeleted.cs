﻿using Hypercube.EventBus.Events;

namespace Hypercube.Shared.Scenes.Events;

public readonly record struct SceneDeleted(Scene Scene) : IEventArgs;