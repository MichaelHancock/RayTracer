﻿namespace RayTracer.Model
{
    public class RayPoint : RayTuple
    {
        public RayPoint(float x, float y, float z)
            : base(x, y, z, W_IDENTIFIER)
        {
        }

        private const float W_IDENTIFIER = 1F;
    }
}
