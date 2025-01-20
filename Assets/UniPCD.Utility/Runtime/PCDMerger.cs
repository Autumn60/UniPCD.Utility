using System.Collections.Generic;
using UnityEngine;

namespace UniPCD.Utility
{
  public static class PCDMerger
  {
    public static bool Merge(PCD[] sources, out PCD merged)
    {
      merged = null;
      if (sources == null || sources.Length <= 1) return false;

      PCD.Header header;
      {
        string header_json = JsonUtility.ToJson(sources[0].header);
        header = JsonUtility.FromJson<PCD.Header>(header_json);
      }
      for (int i = 1; i < sources.Length; i++)
      {
        header.points += sources[i].header.points;
      }
      header.width = header.points;
      merged = new PCD()
      {
        header = header,
        pointCloud = new PointCloud()
      };

      List<Point> points = new List<Point>();
      for (int i = 0; i < sources.Length; i++)
      {
        points.AddRange(sources[i].pointCloud.points);
      }
      merged.pointCloud.points = points.ToArray();
      return true;
    }
  }
}