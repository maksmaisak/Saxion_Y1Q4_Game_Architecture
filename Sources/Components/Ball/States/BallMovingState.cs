﻿using System;
using Engine;
using NUnit.Framework;

namespace Pong
{
    public class BallMovingState : FsmState<Ball>,
        IEventReceiver<PointScoreEvent>,
        IEventReceiver<BallBoostEvent>
    {
        private const float InitialHorizontalSpeed = 200f;
        private const float MaxInitialVerticalSpeed = 70f;

        private Rigidbody rigidbody;
        private bool nextTowardsRight;

        public override void Enter()
        {
            base.Enter();

            rigidbody = rigidbody ?? Get<Rigidbody>();
            Assert.IsNotNull(rigidbody);

            float direction = nextTowardsRight ? 1f : -1f;

            Random random = Services.Get<Random>();
            float verticalSpeedModifier = (float)(random.NextDouble() + 0.5);

            rigidbody.velocity = new Vector2(
                InitialHorizontalSpeed  * direction,
                MaxInitialVerticalSpeed * verticalSpeedModifier
            );
        }

        void OnCollision(Collision collision)
        {
            if (!isEntered) return;

            if (collision.gameObject.Has<Paddle>())
            {
                agent.isBoosting = false;
            }
        }

        public void On(BallBoostEvent eventData)
        {
            if (!isEntered) return;

            if (eventData.ball != gameObject) return;

            agent.isBoosting = true;
        }

        public void On(PointScoreEvent eventData)
        {
            // Make it go towards the loser.
            nextTowardsRight = eventData.leftPlayerScored;
        }
    }
}