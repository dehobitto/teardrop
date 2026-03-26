using System.Numerics;
using Raylib_cs;

using static teardrop.utils.VoxHelper;

Directory.SetCurrentDirectory("../../../../");

var levelPath = "teardrop/data/levels/1.vox";

var data = LoadVox(levelPath);

// Вывод среза в консоль (оставил как было)
for (int z = 0; z < data.Depth; z++)
{
    for (int x = 0; x < data.Width; x++)
    {
        byte voxel = data.voxels[x, 0, z];

        if (voxel == 0)
        {
            Console.Write(" . ");
        }
        else
        {
            Console.Write($"{voxel,2} "); 
        }
    }
    Console.WriteLine();
}

Raylib.InitWindow(1280, 720, "Voxel Engine");
Raylib.DisableCursor();
Raylib.SetTargetFPS(144);

Camera3D camera = new Camera3D();
camera.Position = new Vector3(5.0f, 5.0f, 5.0f);
camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
camera.FovY = 45.0f;
camera.Projection = CameraProjection.Perspective;

while (!Raylib.WindowShouldClose())
{
    Raylib.UpdateCamera(ref camera, CameraMode.FirstPerson);

    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.RayWhite);

    Raylib.BeginMode3D(camera);
    
    Raylib.DrawGrid(16, 1);

    for (int x = 0; x < data.Width; x++)
    {
        for (int y = 0; y < data.Height; y++)
        {
            for (int z = 0; z < data.Depth; z++)
            {
                if (data.voxels[x, y, z] == 0) continue;
                
                DrawCube(x, z, y);
            }
        }
    }

    Raylib.EndMode3D();

    Raylib.DrawFPS(10, 10);

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
static void DrawCube(int x, int y, int z, float size = 1f)
{
    Vector3 position = new Vector3(x, y, z);
    
    // Рисуем сплошной цветной куб
    Raylib.DrawCube(position, size, size, size, Color.Green);
    
    // Рисуем обводку граней (чтобы было видно ребра)
    Raylib.DrawCubeWires(position, size, size, size, Color.DarkGreen);
}