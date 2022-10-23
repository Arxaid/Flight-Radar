// This file is part of the Project RLS.
//
// Copyright (c) 2019 Vladislav Sosedov.

using Models;
using System;
using System.IO;

namespace Functions
{
    class TSimulator
    {
        public double StartTime;
        public double EndTime;
        public double StepTime;
        public double CurrentTime;
        public int AircraftCounter;
        public int MissileCounter;
        public TAircraft[] AircraftArr;
        public TMissile[] MissileArr;
        public TCommandPost CP;
        public TRLS RLS;

        public TSimulator(): this(0.0, 10.0, 0.1) { }
        public TSimulator(double t0, double tk, double dt)
        {
            StartTime = t0;
            EndTime = tk;
            StepTime = dt;
            CurrentTime = t0;
            AircraftCounter = 0;
            MissileCounter = 0;
            AircraftArr = new TAircraft[10];
            MissileArr = new TMissile[10];
        }

        public void ObjectCreator(TargetType.Type CurrentType,
                                  double XCoord, double YCoord, double Velocity,
                                  double Acceleration, double MaxVelocity,
                                  double SafetyDistance, double TargetingDistance,
                                  double StartingTime, double Course) 
        {
            if(CurrentType == TargetType.Type.Unknown)
            {
                
            }
            if(CurrentType == TargetType.Type.CommandPost)
            {
                CP = new TCommandPost();
                CP.SetCurX(XCoord);
                CP.SetCurY(YCoord);
                CP.SetCurTime(StartingTime);
                CP.SetSafetyDistance(SafetyDistance);
            }
            if(CurrentType == TargetType.Type.RLS)
            {
                RLS = new TRLS();
                RLS.SetCurX(XCoord);
                RLS.SetCurY(YCoord);
                RLS.SetCurTime(StartingTime);
                RLS.SetDistance(TargetingDistance);
            }
            if(CurrentType == TargetType.Type.Aircraft)
            {
                AircraftArr[AircraftCounter] = new TAircraft();
                AircraftArr[AircraftCounter].SetCurX(XCoord);
                AircraftArr[AircraftCounter].SetCurY(YCoord);
                AircraftArr[AircraftCounter].SetCurTime(StartingTime);
                AircraftArr[AircraftCounter].SetVel(Velocity);
                AircraftArr[AircraftCounter].SetAngle(Course);

                AircraftCounter++;
            }
            if(CurrentType == TargetType.Type.Missile)
            {
                MissileArr[MissileCounter] = new TMissile();
                MissileArr[MissileCounter].SetCurX(XCoord);
                MissileArr[MissileCounter].SetCurY(YCoord);
                MissileArr[MissileCounter].SetCurTime(StartingTime);
                MissileArr[MissileCounter].SetVel(Velocity);
                MissileArr[MissileCounter].SetAngle(Course);
                MissileArr[MissileCounter].SetAcceleration(Acceleration);
                MissileArr[MissileCounter].SetMaxVelocity(MaxVelocity);

                MissileCounter++;
            }
        }
        
        public void Run(int AircraftCounter, int MissileCounter)
        {
            string path = @"C:\Users\Vallr\source\repos\ProjectSQL\ProjectSQL\Output.txt";
            FileStream MainOutput = new FileStream(path, FileMode.Create);
            StreamWriter Writer = new StreamWriter(MainOutput);


            while(CurrentTime <= EndTime)
            {
                Writer.WriteLine("[" + Math.Round(CurrentTime, 1) + "]");
                for(int i = 0; i < AircraftCounter; i++)
                {
                    AircraftArr[i].Move(CurrentTime);
                    double Distance = Math.Sqrt(Math.Abs((AircraftArr[i].GetCurX() - RLS.GetCurX()) * (AircraftArr[i].GetCurY() - RLS.GetCurY())));
                    if (CPState(Distance, CP.GetSafetyDistance()) == "safe")
                    {
                        Writer.WriteLine("Aircraft #" + (i + 1) + " by Range " + Distance);
                        if (Distance <= RLS.GetDistance())
                        {
                            Writer.WriteLine("Aircraft spotted");
                        }
                    }
                }
                for(int i = 0; i < MissileCounter; i++)
                {
                    MissileArr[i].Move(CurrentTime);
                    double Distance = Math.Sqrt(Math.Abs((MissileArr[i].GetCurX() - RLS.GetCurX()) * (MissileArr[i].GetCurY() - RLS.GetCurY())));
                    if (CPState(Distance, CP.GetSafetyDistance()) == "safe")
                    {
                        Writer.WriteLine("Missile #" + (i + 1) + " by Range " + Distance);
                        if (Distance <= RLS.GetDistance())
                        {
                            Writer.WriteLine("Missile spotted");
                        }
                    }
                }
                CurrentTime += StepTime;
            }
            Writer.Close();
        }
        public string CPState(double Distance, double SafeDistance)
        {
            if(Distance <= SafeDistance)
            {
                return "destroyed";
            }
            else return "safe";
        }
    }
}