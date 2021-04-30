using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The ARKit implementation of the <c>XRPlaneSubsystem</c>. Do not create this directly. Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class ARKitXRPlaneSubsystem : XRPlaneSubsystem
    {
        protected override Provider CreateProvider() => new ARKitProvider();

        class ARKitProvider : Provider
        {
            public override void Destroy() => NativeApi.UnityARKit_Planes_Shutdown();

            public override void Start() =>  NativeApi.UnityARKit_Planes_Start();

            public override void Stop() => NativeApi.UnityARKit_Planes_Stop();

            public override unsafe void GetBoundary(
                TrackableId trackableId,
                Allocator allocator,
                ref NativeArray<Vector2> boundary)
            {
                int numPoints;
                void* verticesPtr;
                void* plane = NativeApi.UnityARKit_Planes_AcquireBoundary(
                    trackableId,
                    out verticesPtr,
                    out numPoints);

                try
                {
                    CreateOrResizeNativeArrayIfNecessary(numPoints, allocator, ref boundary);
                    var transformPositionsHandle = new TransformBoundaryPositionsJob
                    {
                        positionsIn = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector4>(verticesPtr, numPoints, Allocator.None),
                        positionsOut = boundary
                    }.Schedule(numPoints, 1);

                    new FlipBoundaryWindingJob
                    {
                        positions = boundary
                    }.Schedule(transformPositionsHandle).Complete();
                }
                finally
                {
                    NativeApi.UnityARKit_Planes_ReleaseBoundary(plane);
                }
            }

            struct FlipBoundaryWindingJob : IJob
            {
                public NativeArray<Vector2> positions;

                public void Execute()
                {
                    var half = positions.Length / 2;
                    for (int i = 0; i < half; ++i)
                    {
                        var j = positions.Length - 1 - i;
                        var temp = positions[i];
                        positions[i] = positions[j];
                        positions[j] = temp;
                    }
                }
            }

            struct TransformBoundaryPositionsJob : IJobParallelFor
            {
                [ReadOnly]
                public NativeArray<Vector4> positionsIn;

                [WriteOnly]
                public NativeArray<Vector2> positionsOut;

                public void Execute(int index)
                {
                    positionsOut[index] = new Vector2(
                        // NB: https://developer.apple.com/documentation/arkit/arplanegeometry/2941052-boundaryvertices?language=objc
                        // "The owning plane anchor's transform matrix defines the coordinate system for these points."
                        // It doesn't explicitly state the y component is zero, but that must be the case if the
                        // boundary points are in plane-space. Emperically, it has been true for horizontal and vertical planes.
                        // This IS explicitly true for the extents (see above) and would follow the same logic.
                        //
                        // Boundary vertices are in right-handed coordinates and clockwise winding order. To convert
                        // to left-handed, we flip the Z coordinate, but that also flips the winding, so we have to
                        // flip the winding back to clockwise by reversing the polygon index (j).
                         positionsIn[index].x,
                        -positionsIn[index].z);
                }
            }

            public override unsafe TrackableChanges<BoundedPlane> GetChanges(
                BoundedPlane defaultPlane,
                Allocator allocator)
            {
                int addedLength, updatedLength, removedLength, elementSize;
                void* addedArrayPtr, updatedArrayPtr, removedArrayPtr;
                var context = NativeApi.UnityARKit_Planes_AcquireChanges(
                    out addedArrayPtr, out addedLength,
                    out updatedArrayPtr, out updatedLength,
                    out removedArrayPtr, out removedLength,
                    out elementSize);

                try
                {
                    return new TrackableChanges<BoundedPlane>(
                        addedArrayPtr, addedLength,
                        updatedArrayPtr, updatedLength,
                        removedArrayPtr, removedLength,
                        defaultPlane, elementSize,
                        allocator);
                }
                finally
                {
                    NativeApi.UnityARKit_Planes_ReleaseChanges(context);
                }
            }

            public override PlaneDetectionMode requestedPlaneDetectionMode
            {
                get => NativeApi.UnityARKit_Planes_GetRequestedPlaneDetectionMode();
                set => NativeApi.UnityARKit_Planes_SetRequestedPlaneDetectionMode(value);
            }
        }

        /// <summary>
        /// Container to wrap the native ARKit APIs needed at registration.
        /// </summary>
        static class NativeApi
        {
            [DllImport("__Internal")]
            static internal extern unsafe bool UnityARKit_Planes_SupportsClassification();

            [DllImport("__Internal")]
            static internal extern void UnityARKit_Planes_Shutdown();

            [DllImport("__Internal")]
            static internal extern void UnityARKit_Planes_Start();

            [DllImport("__Internal")]
            static internal extern void UnityARKit_Planes_Stop();

            [DllImport("__Internal")]
            static internal extern unsafe void* UnityARKit_Planes_AcquireChanges(
                out void* addedPtr, out int addedLength,
                out void* updatedPtr, out int updatedLength,
                out void* removedPtr, out int removedLength,
                out int elementSize);

            [DllImport("__Internal")]
            static internal extern unsafe void UnityARKit_Planes_ReleaseChanges(void* changes);

            [DllImport("__Internal")]
            static internal extern PlaneDetectionMode UnityARKit_Planes_GetRequestedPlaneDetectionMode();

            [DllImport("__Internal")]
            static internal extern void UnityARKit_Planes_SetRequestedPlaneDetectionMode(PlaneDetectionMode mode);

            [DllImport("__Internal")]
            static internal extern PlaneDetectionMode UnityARKit_Planes_GetCurrentPlaneDetectionMode();

            [DllImport("__Internal")]
            static internal extern unsafe void* UnityARKit_Planes_AcquireBoundary(
                TrackableId trackableId,
                out void* verticiesPtr,
                out int numPoints);

            [DllImport("__Internal")]
            static internal extern unsafe void UnityARKit_Planes_ReleaseBoundary(
                void* boundary);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            if (!Api.AtLeast11_0())
                return;

            var cinfo = new XRPlaneSubsystemDescriptor.Cinfo
            {
                id = "ARKit-Plane",
                subsystemImplementationType = typeof(ARKitXRPlaneSubsystem),
                supportsHorizontalPlaneDetection = true,
                supportsVerticalPlaneDetection = Api.AtLeast11_3(),
                supportsArbitraryPlaneDetection = false,
                supportsBoundaryVertices = true,
                supportsClassification = NativeApi.UnityARKit_Planes_SupportsClassification(),
            };

            XRPlaneSubsystemDescriptor.Create(cinfo);
        }
    }
}
