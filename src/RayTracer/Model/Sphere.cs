using RayTracer.Utility;

namespace RayTracer.Model
{
    public class Sphere : WorldObject
    {
        public RayPoint CentrePoint { get; init; }
        public float Radii { get; init; }
        public Matrix Transform { get; set; }

        public Sphere()
        {
            Transform = Transformation
                .GetIdentiyMatrix(
                    Transformation.TransformationMatrixSize
                );

            // TODO - Assign dynamically
            CentrePoint = new RayPoint(0, 0, 0);
            Radii = 1;
        }

        public Intersection[] GetIntersects(Ray ray) => ray.GetIntersects(this);

        public RayVector GetSurfaceNormalAt(RayPoint point) =>
            (point.Subtract(new RayPoint(0, 0, 0)) as RayVector).Normalise();
    }
}
