using Microsoft.Xna.Framework;

namespace ITW {
	interface IMovable {
		void SetVelocity(float x, float y);
		Vector2 GetVelocity();
		void MoveTick();
		void MoveBy(float x, float y);
		void MoveBy(Vector2 v);
		Point GetPosition();
	}
}