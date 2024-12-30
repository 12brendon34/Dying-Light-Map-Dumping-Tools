# Dying Light Map Dumping Tools
 Some tools (and scripts) for Dying Light Map Dumping, Made for my entertainment and modding.

## EDS Files

EDS files represent grouped objects in the Chrome Engine map editor, ChromED.

- The `Map2EDS` program generates EDS files from raw text, specifically for `ModelObjects` extracted from `SOBJ` files.
- xModel2Map.py can also be used for converting Call of Duty xModel JSON files, Tested with Terminal from MW3 (2009), partially
- While other object classes might work, they have not been tested.

## SOBJ Files

`SOBJ` files store data for static mesh elements in Chrome Engine-based games.

 **Magic ID**: The identifier `SO16` is used in Dying Light 1 and other Chrome Engine 6 games, including:
  - Dying Light: Bad Blood
  - FIM Speedway Grand Prix 15
  - Dead Island / Riptide: Definitive Edition

A `SOBJ` file has two sections of importance:

1. **Mesh Section**
   - References the mesh to be loaded from the game's RPACK (Resource Pack) format.

2. **Entity Section**
   - Includes data such as:
     - Position
     - Scale
     - Quaternion rotation
     - Both mesh colors (in BGRA format)
     - A value indicating which mesh to use

### Additional Notes:
- Compiled terrain is divided into chunks and stored within `SOBJ` files.
(Excluding Mesh Data which is stored in the `RPACK`)
- When reimported into the editor, terrain chunks behave as standard `SimpleObjects` rather than editable terrain.
- The SOBJ Dumper outputs to the console, catch it with `> map.txt`
