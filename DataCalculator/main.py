import os

from Walk import WalkSequence, WalkCharacteristics

TRACKS_FOLDER = "Tracks"
def load_all_walk_sequences() -> list[WalkSequence]:
    walks = []
    for directory in os.listdir(TRACKS_FOLDER):
        walk_sequence_directory = os.path.join(TRACKS_FOLDER, directory)
        walks.append(WalkSequence(walk_sequence_directory))
    return walks



sequences = load_all_walk_sequences()

total_walk_characteristic = WalkCharacteristics()
for sequence in sequences:
    characteristic = sequence.calculate_walk_characteristics()
    characteristic.calculate()
    print(sequence.person)
    print(characteristic)


    total_walk_characteristic.concat(characteristic)

total_walk_characteristic.calculate()

print(total_walk_characteristic)