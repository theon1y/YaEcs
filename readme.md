# Yet Another Entity Component System
This is just a simple naive implementation of a ECS pattern:

Entities are ids, Components are data classes, and Systems are stateless
pieces of logic.

Everything is living in a World that can be initialized, updated and
disposed externally.

The update is broken down into steps that are held in a
UpdateStepRegistry and is easily customizable.
