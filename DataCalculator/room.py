import json
import numpy as np

import record


def point_to_segment_distance(p, a, b) -> float:
    """
    Compute distance from point p to segment ab.
    p, a, b are numpy arrays of shape (2,) or (3,).
    """
    # Vector from a to b
    ab = b - a
    # Vector from a to p
    ap = p - a

    # Project ap onto ab, normalized by |ab|^2
    t = np.dot(ap, ab) / np.dot(ab, ab)

    # Clamp t to [0, 1] to stay within the segment
    t = max(0, min(1, t))

    # Closest point on segment
    closest = a + t * ab

    return np.linalg.norm(p - closest)


def point_to_polyline_distance(p, polyline):
    """
    Compute minimum distance from point p to polyline.
    p is (2,), polyline is array-like of shape (N,2).
    """
    p = np.array(p, dtype=float)
    polyline = np.array(polyline, dtype=float)

    distances = [
        point_to_segment_distance(p, polyline[i], polyline[i + 1])
        for i in range(len(polyline) - 1)
    ]

    return min(distances)


class Room:
    def __init__(self, filepath):
        with open(filepath) as file:
            file_content = json.load(file)

            self.room_name = file_content["roomName"]
            self.records = record.create_records_array(file_content["records"])
            self.points: list[tuple[float, float]] = []

            for json_point in file_content['points']:
                self.points.append((json_point['x'], json_point['y']))


    def get_velocity_array_in_zone(self) -> list[float]:
        velocity : list[ float] = []
        for record in self.records:
            if record.in_control_zone:
                vel =  np.linalg.norm(record.vel)
                velocity.append(vel)
        return velocity


    def get_velocity_array_outside_zone(self) -> list[float]:
        velocity: list[ float] = []
        for record in self.records:
            if not record.in_control_zone:
                vel = np.linalg.norm(record.vel)
                velocity.append(vel)
        return velocity


    def get_offset_array_in_zone(self) -> list[float]:
        offsets = []
        for record in self.records:
            if record.in_control_zone:
                offsets.append(
                    point_to_polyline_distance(record.pos,self.points)
                )
        return offsets


    def get_offset_array_outside_zone(self) -> list[float]:
        offsets = []
        for record in self.records:
            if not record.in_control_zone:
                offsets.append(
                    point_to_polyline_distance(record.pos, self.points)
                )
        return offsets