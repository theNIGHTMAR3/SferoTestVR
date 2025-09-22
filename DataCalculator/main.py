import os
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.ticker import MultipleLocator

from Walk import WalkSequence, WalkCharacteristics

VELOCITY_DELTA = 0.1

TRACKS_FOLDER = "Tracks"
def load_all_walk_sequences() -> list[WalkSequence]:
    walks = []
    for directory in os.listdir(TRACKS_FOLDER):
        walk_sequence_directory = os.path.join(TRACKS_FOLDER, directory)
        walks.append(WalkSequence(walk_sequence_directory))
    return walks


def plot_velocities(vels:list[float], title: str) -> None:
    bins = np.arange(0, np.max(vels), VELOCITY_DELTA)
    counts, edges = np.histogram(vels, bins=bins)
    # X values: midpoints of bins
    x = (edges[:-1] + edges[1:]) / 2

    # Plot
    plt.gca().xaxis.set_major_locator(MultipleLocator(0.5))
    plt.bar(edges[:-1], counts, width=VELOCITY_DELTA, align="edge", edgecolor="black")
    plt.xlabel("Prędkość sfery rzeczywistej [m/s]")
    plt.ylabel("Liczba pomiarów")
    plt.title(title)
    plt.grid(True)
    plt.show()


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

plot_velocities(total_walk_characteristic.speed_outside_zone, "Histogram prędkości sfery poza strefą kontroli")
plot_velocities(total_walk_characteristic.speed_in_zone, "Histogram prędkości sfery w strefie kontroli")

