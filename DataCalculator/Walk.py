import os
import statistics
from room import Room

class WalkCharacteristics:
    def __init__(self):
        self.in_zone_avg_walk_speed = 0
        self.in_zone_dev_walk_speed = 0
        self.in_zone_max_walk_speed = 0
        self.in_zone_avg_offset = 0
        self.in_zone_dev_offset = 0

        self.outside_zone_avg_walk_speed = 0
        self.outside_zone_dev_walk_speed = 0
        self.outside_zone_max_walk_speed = 0
        self.outside_zone_avg_offset = 0
        self.outside_zone_dev_offset = 0

        self.speed_in_zone = []
        self.speed_outside_zone = []

        self.offset_in_zone = []
        self.offset_outside_zone = []

    def append(
            self,
            new_speed_in_zone,
            new_speed_outside_zone,
            new_offset_in_zone,
            new_offset_outside_zone,
    ) -> None:
        self.speed_in_zone += new_speed_in_zone
        self.speed_outside_zone += new_speed_outside_zone

        self.offset_in_zone += new_offset_in_zone
        self.offset_outside_zone += new_offset_outside_zone

    def concat(
            self,
            other
    ) -> None:
        self.speed_in_zone += other.speed_in_zone
        self.speed_outside_zone += other.speed_outside_zone

        self.offset_in_zone += other.offset_in_zone
        self.offset_outside_zone += other.offset_outside_zone

    def calculate(self) -> None:
       self._calculate_in_zone()
       self._calculate_outside_zone()

    def _calculate_in_zone(self) -> None:
        if len(self.offset_in_zone)>0:
            self.in_zone_avg_walk_speed = sum(self.speed_in_zone) / len(self.speed_in_zone)
            self.in_zone_dev_walk_speed = statistics.stdev(self.speed_in_zone)
            self.in_zone_max_walk_speed = max(self.speed_in_zone)
            self.in_zone_avg_offset =   sum(self.offset_in_zone) / len(self.offset_in_zone)
            self.in_zone_dev_offset = statistics.stdev(self.offset_in_zone)
        else:
            self.in_zone_avg_walk_speed = 0
            self.in_zone_dev_walk_speed = 0
            self.in_zone_max_walk_speed = 0
            self.in_zone_avg_offset = 0
            self.in_zone_dev_offset = 0

    def _calculate_outside_zone(self) -> None:
            self.outside_zone_avg_walk_speed = sum(self.speed_outside_zone) / len(self.speed_outside_zone)
            self.outside_zone_dev_walk_speed = statistics.stdev(self.speed_outside_zone)
            self.outside_zone_max_walk_speed = max(self.speed_outside_zone)
            self.outside_zone_avg_offset = sum(self.offset_outside_zone) / len(self.offset_outside_zone)
            self.outside_zone_dev_offset = statistics.stdev(self.offset_outside_zone)

    def remove_player_stops(self) -> None:
        # remove the records where player wasn't moving

        # iterate from the end
        for i in range(len(self.speed_outside_zone) - 1, -1, -1):
            if self.speed_outside_zone[i] <= 0.05:
                del self.speed_outside_zone[i]
                del self.offset_outside_zone[i]


    def __str__(self):
        return f"""
    in_zone_avg_walk_speed = {self.in_zone_avg_walk_speed}
    in_zone_dev_walk_speed = {self.in_zone_dev_walk_speed}
    in_zone_max_walk_speed = {self.in_zone_max_walk_speed}
    in_zone_avg_offset = {self.in_zone_avg_offset}
    in_zone_dev_offset = {self.in_zone_dev_offset}
    
    outside_zone_avg_walk_speed = {self.outside_zone_avg_walk_speed}
    outside_zone_dev_walk_speed = {self.outside_zone_dev_walk_speed}
    outside_zone_max_walk_speed = {self.outside_zone_max_walk_speed}
    outside_zone_avg_offset = {self.outside_zone_avg_offset}
    outside_zone_dev_offset = {self.outside_zone_dev_offset}
        """

class WalkSequence:
    def __init__(self, sequence_path):

        self.rooms: list[Room] = []
        self.person = os.path.basename(sequence_path)
        for filename in os.listdir(sequence_path):
            if not filename.endswith(".svg"):
                filepath  = os.path.join(sequence_path, filename)
                self.rooms.append(Room(filepath))

    def calculate_walk_characteristics(self) -> WalkCharacteristics:
        speed_array_in_zone: list[float] = [speed for room in self.rooms for speed in room.get_velocity_array_in_zone()]
        offset_array_in_zone = [offset for room in self.rooms for offset in room.get_offset_array_in_zone()]

        speed_array_outside_zone: list[float] = [speed for room in self.rooms for speed in room.get_velocity_array_outside_zone()]
        offset_array_outside_zone = [offset for room in self.rooms for offset in room.get_offset_array_outside_zone()]

        characteristics = WalkCharacteristics()
        characteristics.append(speed_array_in_zone,speed_array_outside_zone,offset_array_in_zone,offset_array_outside_zone)
        characteristics.remove_player_stops()
        return characteristics