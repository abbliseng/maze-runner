using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maze : MonoBehaviour
{
    public RawImage debug_maze_texture;
    public Vector2 maze_size;
    public Texture2D maze_texture;
    
    // private List<Vector2> walls; 
    // private char[,] generated_maze;
    
    public void TestMazeGenerator()
    {
        debug_maze_texture.texture = GenerateMazeTexture(GenerateMaze((int)maze_size.x, (int)maze_size.y));
    }

    public Texture2D GenerateMazeTexture(char[,] maze)
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        maze_texture = new Texture2D(width, height);
        maze_texture.filterMode = FilterMode.Point;
        // Generate maze :)
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (maze[i,j] == 'u')
                {
                    maze_texture.SetPixel(i, j, Color.red);
                }
                else if (maze[i,j] == 'w')
                {
                    maze_texture.SetPixel(i, j, Color.black);
                } else if (maze[i, j] == 'c')
                {
                    maze_texture.SetPixel(i, j, Color.white);
                } else if (maze[i, j] == 's')
                {
                    maze_texture.SetPixel(i, j, Color.green);
                }
                else if (maze[i, j] == 'e')
                {
                    maze_texture.SetPixel(i, j, Color.blue);
                }
            }
        }
        maze_texture.Apply();
        return maze_texture;
    }

    public char[,] GenerateMaze(int width, int height)
    {
        char[,] generated_maze = new char[width, height];
        // Start the grid full of unvisited tiles ('u')
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                generated_maze[i, j] = 'u';
            }
        }
        // pick random spot to start maze at
        int starting_height = Random.Range(1, height-1);
        int starting_width = Random.Range(1, width-1);
        // mark spot as path and add surrounding walls to wall list/array
        List<Vector2> walls = new List<Vector2>();
        generated_maze[starting_width, starting_height] = 'c';
        walls.Add(new Vector2(starting_width - 1, starting_height));
        walls.Add(new Vector2(starting_width, starting_height - 1));
        walls.Add(new Vector2(starting_width, starting_height + 1));
        walls.Add(new Vector2(starting_width + 1, starting_height));
        // denote the above walls as walls
        generated_maze[starting_width - 1, starting_height] = 'w';
        generated_maze[starting_width, starting_height - 1] = 'w';
        generated_maze[starting_width, starting_height + 1] = 'w';
        generated_maze[starting_width + 1, starting_height] = 'w';
        // step 3, algorithm
        // while walls pick random wall
        while (walls.Count > 0)
        {
            // pick random wall from list
            Vector2 random_wall = walls[(int)Random.Range(0, walls.Count - 1)];
            int x = (int)random_wall.x;
            int y = (int)random_wall.y;

            // Check if its a left wall
            if (y != 0)
            {
                // if (maze[rand_wall[0]][rand_wall[1]-1] == 'u' and maze[rand_wall[0]][rand_wall[1]+1] == 'c'):
                if (generated_maze[x, y - 1] == 'u' && generated_maze[x, y + 1] == 'c')
                {
                    int s_cells = SurroundingCells(random_wall, generated_maze);
                    if (s_cells < 2)
                    {
                        generated_maze[x, y] = 'c';
                        // Mark new walls
                        // Upper cell
                        if (x != 0)
                        {
                            if (generated_maze[x - 1, y] != 'c')
                            {
                                generated_maze[x - 1, y] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x - 1, y)))
                            {
                                walls.Add(new Vector2(x - 1, y));
                            }
                        }
                        // Bottom cell
                        if (x != (width - 1))
                        {
                            if (generated_maze[x + 1, y] != 'c')
                            {
                                generated_maze[x + 1, y] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x + 1, y)))
                            {
                                walls.Add(new Vector2(x + 1, y));
                            }
                        }
                        // Leftmost wall
                        if (y != 0)
                        {
                            if (generated_maze[x, y - 1] != 'c')
                            {
                                generated_maze[x, y - 1] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x, y - 1)))
                            {
                                walls.Add(new Vector2(x, y - 1));
                            }
                        }
                    }
                    walls.Remove(new Vector2(x, y));
                    continue;
                }
            }
            // Check if its an upper wall
            if (x != 0)
            {
                if (generated_maze[x - 1, y] == 'u' && generated_maze[x + 1, y] == 'c')
                {
                    int s_cells = SurroundingCells(random_wall, generated_maze);
                    if (s_cells < 2)
                    {
                        generated_maze[x, y] = 'c';
                        // Upper cell
                        if (x != 0)
                        {
                            if (generated_maze[x - 1, y] != 'c')
                            {
                                generated_maze[x - 1, y] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x - 1, y)))
                            {
                                walls.Add(new Vector2(x - 1, y));
                            }
                        }
                        // Leftmost wall
                        if (y != 0)
                        {
                            if (generated_maze[x, y - 1] != 'c')
                            {
                                generated_maze[x, y - 1] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x, y - 1)))
                            {
                                walls.Add(new Vector2(x, y - 1));
                            }
                        }
                        // Rightmost cell
                        if (y != (height - 1))
                        {
                            if (generated_maze[x, y + 1] != 'c')
                            {
                                generated_maze[x, y + 1] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x, y + 1)))
                            {
                                walls.Add(new Vector2(x, y + 1));
                            }
                        }
                    }
                    walls.Remove(new Vector2(x, y));
                    continue;
                }
            }
            // Check the bottom wall
            if (x != (width - 1))
            {
                if (generated_maze[x + 1, y] == 'u' && generated_maze[x - 1, y] == 'c')
                {
                    int s_cells = SurroundingCells(random_wall, generated_maze);
                    if (s_cells < 2)
                    {
                        generated_maze[x, y] = 'c';
                        // Mark new walls
                        // Bottom cell
                        if (x != (width - 1))
                        {
                            if (generated_maze[x + 1, y] != 'c')
                            {
                                generated_maze[x + 1, y] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x + 1, y)))
                            {
                                walls.Add(new Vector2(x + 1, y));
                            }
                        }
                        // Leftmost wall
                        if (y != 0)
                        {
                            if (generated_maze[x, y - 1] != 'c')
                            {
                                generated_maze[x, y - 1] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x, y - 1)))
                            {
                                walls.Add(new Vector2(x, y - 1));
                            }
                        }
                        // Rightmost cell
                        if (y != (height - 1))
                        {
                            if (generated_maze[x, y + 1] != 'c')
                            {
                                generated_maze[x, y + 1] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x, y + 1)))
                            {
                                walls.Add(new Vector2(x, y + 1));
                            }
                        }
                    }
                    walls.Remove(new Vector2(x, y));
                    continue;
                }
            }
            // Check the right wall
            if (y != (height - 1))
            {
                if (generated_maze[x, y + 1] == 'u' && generated_maze[x, y - 1] == 'c')
                {
                    int s_cells = SurroundingCells(random_wall, generated_maze);
                    if (s_cells < 2)
                    {
                        generated_maze[x, y] = 'c';
                        // Mark the new walls
                        // Rightmost cell
                        if (y != (height - 1))
                        {
                            if (generated_maze[x, y + 1] != 'c')
                            {
                                generated_maze[x, y + 1] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x, y + 1)))
                            {
                                walls.Add(new Vector2(x, y + 1));
                            }
                        }
                        // Bottom cell
                        if (x != (width - 1))
                        {
                            if (generated_maze[x + 1, y] != 'c')
                            {
                                generated_maze[x + 1, y] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x + 1, y)))
                            {
                                walls.Add(new Vector2(x + 1, y));
                            }
                        }
                        // Upper cell
                        if (x != 0)
                        {
                            if (generated_maze[x - 1, y] != 'c')
                            {
                                generated_maze[x - 1, y] = 'w';
                            }
                            if (!walls.Contains(new Vector2(x - 1, y)))
                            {
                                walls.Add(new Vector2(x - 1, y));
                            }
                        }
                    }
                    walls.Remove(new Vector2(x, y));
                    continue;
                }
            }
            walls.Remove(new Vector2(x, y));
        }
        // Mark remaining unvisited cells as walls
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (generated_maze[i, j] == 'u')
                {
                    generated_maze[i, j] = 'w';
                }
            }
        }
        // Set entrance and exit (maybe randomize which side the entence and exists should be :)
        for (int i = 0; i < width; i++)
        {
            if (generated_maze[i, (height-2)] == 'c')
            {
                //generated_maze[i, (height - 1)] = 's';
                generated_maze[i, (height - 2)] = 's';
                break;
            }
        }
        for (int i = (width - 1); i >= 0; i--)
        {
            if (generated_maze[i, 1] == 'c')
            {
                // generated_maze[i, 0] = 'e';
                generated_maze[i, 1] = 'e';
                break;
            }
        }

        return generated_maze;
    }

    public int SurroundingCells(Vector2 rand_wall, char[,] maze)
    {
        int s_cells = 0;
        if (maze[(int)rand_wall.x - 1, (int)rand_wall.y] == 'c')
        {
            s_cells += 1;
        }
        if (maze[(int)rand_wall.x + 1, (int)rand_wall.y] == 'c')
        {
            s_cells += 1;
        }
        if (maze[(int)rand_wall.x, (int)rand_wall.y - 1] == 'c')
        {
            s_cells += 1;
        }
        if (maze[(int)rand_wall.x, (int)rand_wall.y + 1] == 'c')
        {
            s_cells += 1;
        }
        return s_cells;
    }
}
