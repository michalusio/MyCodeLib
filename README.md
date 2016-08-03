# MyCodeLib
My own library of useful code and functions
It consists of five modules:
- Data Structures:
  - Fully functional min-max heap structure (Used in implementation of A* and Dijkstra's algorithms)
  - Functional discrete sets structure (with optimized Union-Find algorithm)
- Functions:
  - Efficient and expandable RPN (reverse polish notation) parser and solver
  - Expression Tree structure for simplifying equations
  - Derivative algorithm for computing analytical derivatives from Expression Tree or RPN parser.
- PathFinding:
  - Interfaces to standarize any graph concept to algorithms in this section.
  - Created special class for quick start in 2d grid graph (Manhattan2DNode)
  - A* Algorithm implementation for finding the quickest route from A to B
    * Optimized using HashSets instead of lists
    * Optimized using Heap for Open set to be even more efficient
    * Uses generics to adapt to any needs
    * Can be used to find route to any of goals in the set
  - Dijkstra's Algorithm implementation for finding the nearest node meeting given predicate
    * Based on A* implementation (and using it's optimizations)
    * Possibility of specifying distance limit for search
  - Area recognization algorithm for 2D grids using Flood Fill
    * Using iteration instead of recursion, guaranting stability even for big grids
    * Id byte property allowing up to 255 different cell types
    * O(n) time and space complexity
- Plotting:
  - Classes for plotting complex graphs in 2D and 3D using GDI's Bitmap class
    * Specifying formula in form f(x)=y and f(x,y)=z
    * Specifying parametric formulas for x(t) and y(t) ( and z(t) )
    * Specyfing true/false formula for entire regions of space
  - Transformations - interface allowing you to transform points on plots
    * Invertible and Noninvertible transformations interfaces
    * Ready classes with most common transformations (scaling, translation, rotation etc.)
    * Some advanced transformations (Mobius, Spherical, Normalization)
    * Transformation lists to effectively use a series of transforms
    * All transformations are calculated using multi-threading, guaranting fast work
  - Projections, allowing you to convert your plots from 2D to 3D and vice-versa
  - Figures class to plot simple polygons on plots (cubes, spheres, etc.)
- Other:
  - Bmp class as alternative for GDI's SaveToBitmap function
  - Hashing class using Pearson hashing as the base, allowing you to make hashes in byte, short, int and long formats
  - ISendable interface for Net server-client system
  - Logger class with automatic date-time addition, console writing, log saving and deep-log functionality
  - MMath static class with useful functions:
    * Converting from HSV colors to RGB
    * Clamping ints/floats to range
    * Quickly checking if object is Numeric
    * Cutting bytes from byte array
    * Extracting null-terminated 2-byte-char strings from byte array
    * Function to return 2 to the power of n (allowed negative numbers)
    * Calculating Modulo of two double numbers
    * Random numbers with gaussian (normal) distribution
    * Function for threading-safe double/float addition
  - Net Server/Client:
    * Full socket and connection handling
    * Server can manage connection to multiple clients
    * Sending and receiving simple data types (from byte to double)
    * Sending string as byte arrays
    * Sending raw byte arrays
    * Sending objects using ISendable interface
    * Server has the ability to send data to all connected clients

This library will be updated and expanded as my experience in programming gets higher.
