using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBJParser
{

    public class ParseObj
    {


        public static List<byte[]> ParsedObject(string fileName)
        {
            List<byte[]> result = new List<byte[]>();

            int countVertexes = 0;

            string sLine = "";
            ArrayList arrText = new ArrayList();

            ArrayList vertexModel = new ArrayList();
            ArrayList facesModel = new ArrayList();
            
            StreamReader objReader = new StreamReader(fileName);

            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    arrText.Add(sLine);
            }
            objReader.Close();


            //получаем и укладываем в список вертексы модели
            foreach (string line in arrText)
            {
                if (line.Contains("v "))
                {

                    string[] splitedLine = line.Split(' ');
                    splitedLine = splitedLine.Where(a => a != "v").ToArray();

                    float[] convertedLine = new float[splitedLine.Length];
                    for (int i = 0; i < splitedLine.Length; i++)
                    {
                        convertedLine[i] = float.Parse(splitedLine[i]);
                    }
                    vertexModel.Add(convertedLine);
                }
            }

            Face face;
            Face[] faces;
            //получаем и укладываем в список вертексы модели
            foreach (string line in arrText)
            {
                if (line.Contains("f "))
                {

                    string[] splitedLine = line.Split(' ');
                    splitedLine = splitedLine.Where(a => a != "f").ToArray();
                    faces = new Face[splitedLine.Length];

                    //string splitedItem = string.Empty;
                    for (int i = 0; i < splitedLine.Length; i++)

                    {
                        string[] splitedItem = splitedLine[i].Split('/');

                        face = new Face();
                        face.vertex = Convert.ToInt32(splitedItem[0]);
                        face.normal = Convert.ToInt32(splitedItem[1]);

                        faces[i] = face;
                    }

                    facesModel.Add(faces);
                }
            }

            //счетчик вершин
            foreach (Face[] facesForCount in facesModel)
            {
                foreach (Face faceForCount in facesForCount)
                {
                    countVertexes++;
                }
            }

            float[] vertexArray  = new float[countVertexes * 3];
            int count = 0;
            //собираем массив флоатов с вершинами для построения модели
            foreach (Face[] facesForCount in facesModel)
            {
                foreach (Face faceForCount in facesForCount)
                {
                    int positionVertex = faceForCount.vertex;
                    float[] coordinateVertex = (float[])vertexModel[positionVertex - 1];

                    foreach (float item in coordinateVertex)
                    {
                        vertexArray[count] = item;

                        count++;
                    }
                }
            }

            byte[] byteArray = new byte[vertexArray.Length * 4];
            Buffer.BlockCopy(vertexArray, 0, byteArray, 0, byteArray.Length);
            result.Add(byteArray);

            return result;
        }

    }
}
