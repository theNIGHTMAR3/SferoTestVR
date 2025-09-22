class Record:
    def __init__(
            self,
            pos: tuple[float, float],
            vel: float,
            in_control_zone: bool
    ):
        self.pos = pos
        self.vel = vel
        self.in_control_zone = in_control_zone



def create_records_array(json_records) -> list[Record]:
    records = []

    for record in json_records:
        records.append(
            Record(
                (record['pos']['x'],record['pos']['y']),
                (record['sphereRecord']['velocity']),
                record['inControlZone']
            )
        )

    return records