using Raylib_cs;

namespace teardrop.utils;

public class VoxModel
{
    public byte[,,] voxels;

    public int Width;
    public int Height;
    public int Depth;

    private Color[] pallete = new Color[255];

    public VoxModel(int width, int height, int depth)
    {
        this.Width = width;
        this.Height = height; 
        this.Depth = depth;
        
        this.voxels = new byte[this.Width, this.Height, this.Depth];
    }

    public void AddVoxel(byte x, byte y, byte z, byte colorIndex)
    {
        this.voxels[x, y, z] = colorIndex;
    }
}

public class VoxHelper
{
    public static VoxModel? LoadVox(string filePath)
    {
        VoxModel? vox = null;
        
        using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
        {
            string signature = new string(reader.ReadChars(4));
        
            if (signature != "VOX ")
            {
                return null; // not a .vox file
            }
        
            int version = reader.ReadInt32(); // js skip

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                // read chunk
                var chunkId = new string(reader.ReadChars(4));
                var chunkSize = reader.ReadInt32();
                var childrenSize = reader.ReadInt32();
            
                if (chunkId == "MAIN") {continue;}
                
                if (chunkId == "SIZE")
                {
                    int sizeX = reader.ReadInt32();
                    int sizeY = reader.ReadInt32();
                    int sizeZ = reader.ReadInt32();

                    vox = new VoxModel(sizeX, sizeY, sizeZ);
                }
                else if (chunkId == "XYZI")
                {
                    int numVoxels = reader.ReadInt32();

                    // Запускаем цикл по всем вокселям
                    for (int i = 0; i < numVoxels; i++)
                    {
                        byte x = reader.ReadByte();
                        byte y = reader.ReadByte();
                        byte z = reader.ReadByte();
                        byte colorIndex = reader.ReadByte();

                        vox.AddVoxel(x, y, z, colorIndex);
                    }
                }
                else
                {
                    reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
                }
            }
        
            return vox;
        }
    }
}