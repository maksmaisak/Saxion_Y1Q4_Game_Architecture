﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using NUnit.Framework;

namespace Pong
{
    public class Ball : Component, 
        IEventReceiver<PointScoreEvent>
    {
        private const float MaxSpeed = 300f;

        public Rigidbody rigidbody { get; private set; }
        public BallBoostEffectRenderer boostEffectRenderer { get; private set; }

        public FiniteStateMachine<Ball> fsm { get; private set; }

        void Start()
        {
            Add<BallMovingState>();
            fsm = new FiniteStateMachine<Ball>(this);

            rigidbody = Get<Rigidbody>();
            Assert.IsNotNull(rigidbody);

            boostEffectRenderer = GetOrAdd<BallBoostEffectRenderer>();

            Reset();
        }

        void Update()
        {
            rigidbody.velocity = rigidbody.velocity.TruncatedBy(MaxSpeed);
        }

        public void On(PointScoreEvent pointScore)
        {
            Reset();
        }

        private void Reset()
        {
            gameObject.position = game.size * 0.5f;
            rigidbody.velocity = Vector2.zero;

            fsm.ChangeState<BallStunnedState>();
        }
    }
}
