// This file is part of the Project RLS.
//
// Copyright (c) 2019 Vladislav Sosedov.

using System;

namespace Models
{

    struct TPoint { public double X; public double Y; }
    public class TargetType
    {
       public enum Type { Unknown, CommandPost, RLS, Aircraft, Missile }
    }

    abstract class TPosObject
    {
        protected TPoint InitPosition;
        protected TPoint CurPosition;
        protected double CurTime;
        public TPosObject() : this(0, 0, 0) { }
        public TPosObject(double x, double y, double t)
        {
            InitPosition.X = x;
            InitPosition.Y = y;
            CurPosition.X = InitPosition.X;
            CurPosition.Y = InitPosition.Y;
            CurTime = t;
        }

        public double GetInitX() { return InitPosition.X; }
        public double GetInitY() { return InitPosition.Y; }
        public double GetCurX() { return CurPosition.X; }
        public void SetCurX(double xx) { CurPosition.X = xx; }
        public double GetCurY() { return CurPosition.Y; }
        public void SetCurY(double yy) { CurPosition.Y = yy; }
        public double GetCurTime() { return CurTime; }
        public void SetCurTime(double tt) { CurTime = tt; }
        
        public virtual void Move(double t) { }
    }

    class TCommandPost : TPosObject
    {
        protected double SafetyDistance;
        public TCommandPost() : base() { SafetyDistance = 0; }
        public TCommandPost(double x, double y, double t, double sd) : base(x, y, t) { SafetyDistance = sd; }

        public double GetSafetyDistance() { return SafetyDistance; }
        public void SetSafetyDistance(double sd) { SafetyDistance = sd; }

        public override void Move(double t) { base.Move(t); }
    }

    class TTarget : TPosObject
    {
        protected double Velocity;
        protected double Angle;
        public TTarget() : base() { Velocity = 0; Angle = 0; }
        public TTarget(double x, double y, double t, double v, double k) : base(x, y, t) { Velocity = v; Angle = k; }
        public double GetVel() { return Velocity; }
        public void SetVel(double vv) { Velocity = vv; }
        public double GetAngle() { return Angle; }
        public void SetAngle(double kk) { Angle = kk; }

        public override void Move(double t) { base.Move(t); }
    }

    class TAircraft : TTarget
    {
        public TAircraft() : base() { }
        public TAircraft(double x, double y, double t, double v, double k) : base(x, y, t, v, k) { }

        public override void Move(double t)
        {
            SetCurX(GetCurX() - GetVel() * Math.Cos(GetAngle()) * (t - GetCurTime()));
            SetCurY(GetCurY() - GetVel() * Math.Sin(GetAngle()) * (t - GetCurTime()));
        }
    }

    class TMissile : TTarget
    {
        protected double Acceleration;
        protected double MaxVelocity;

        public TMissile() : base()
        {
            Acceleration = 0;
            MaxVelocity = 10;
        }
        public TMissile(double x, double y, double t, double v, double k, double n, double maxvv) : base(x, y, t, v, k)
        {
            Acceleration = n;
            MaxVelocity = maxvv;
        }
        public double GetAcceleration() { return Acceleration; }
        public void SetAcceleration(double nn) { Acceleration = nn; }
        public double GetMaxVelocity() { return MaxVelocity; }
        public void SetMaxVelocity(double maxvv) { MaxVelocity = maxvv;}

        public override void Move(double t)
        {
            SetCurX(GetCurX() - (GetVel() + Acceleration * (t - GetCurTime())) * Math.Cos(GetAngle()) * (t - GetCurTime()));
            SetCurY(GetCurY() - (GetVel() + Acceleration * (t - GetCurTime())) * Math.Sin(GetAngle()) * (t - GetCurTime()));
        }
    }

    class TRLS : TPosObject
    {
        protected double TargetingRange;
        protected double TargetDistance;
        protected double TargetAzimuth;

        public TRLS() : base()
        {
            TargetingRange = 100;
            TargetDistance = 0;
            TargetAzimuth = 0;
        }
        public TRLS(double x, double y, double t, double targetingrange) : base(x, y, t)
        {
            TargetingRange = targetingrange;
            TargetDistance = 0;
            TargetAzimuth = 0;
        }
        public double GetDistance() { return TargetDistance; }
        public void SetDistance(double d) { TargetDistance = d; }
        public double GetAzimuth() { return TargetAzimuth; }
        public void SetAzimuth(double az) { TargetAzimuth = az; }
        public override void Move(double t) { base.Move(t); }
    }
}