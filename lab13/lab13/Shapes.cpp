#include "shapes.h"

const std::vector<GLfloat> tetrafigure {
    -1.0f, -1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
        -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,

        -1.0f, -1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
        -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,

        -1.0f, -1.0f, -1.0f, 0.0f, 0.0f, 1.0f,
        1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
        1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,

        1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
        1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,
        -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
};

   
const std::vector<GLfloat> cubefigure{
    //позиции             //цвета            //текстурные координаты
    -1.0f,  1.0f, 1.0f,   0.0f, 1.0f, 0.0f,  0.0f, 0.1f,  
     1.0f,  1.0f, 1.0f,   1.0f, 0.0f, 0.0f,  1.0f, 1.0f,
     1.0f, -1.0f, 1.0f,   1.0f, 1.0f, 0.0f,  1.0f, 0.0f,
     -1.0f,-1.0f, 1.0f,   1.0f, 0.0f, 1.0f,  0.0f, 0.0f,

     -1.0f, 1.0f, -1.0f, 0.5f, 0.5f, 0.5f, 0.0f, 0.1f,
     -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
     -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
     -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f,

     1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.1f,
     -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
     -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
     1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,

     1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.1f,
     1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
     1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f,
     1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,

     1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.1f,
     -1.0f, 1.0f, -1.0f, 0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
     -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f,
     1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f,

     1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.1f,
     -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
     -1.0f, 1.0f, -1.0f, 0.5f, 0.5f, 0.5f, 1.0f, 0.0f,
     1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f,

};




std::vector<GLfloat>  ParseObjFromFile(const std::string& filePath) {
    std::ifstream objFile(filePath);
    std::vector<GLfloat> loadedVertices; // Хранение загруженных данных
    if (!objFile.is_open()) 
    {
        std::cerr << "Failed to open .obj file: " << filePath << std::endl;
        
        return std::vector<GLfloat>();
    }

    std::vector<glm::vec3> positions;
    std::vector<glm::vec2> texCoords;
    std::vector<glm::vec3> normals;



    std::string line;
    while (std::getline(objFile, line)) {
        std::istringstream ss(line);
        std::string prefix;
        ss >> prefix;

        if (prefix == "v") { // Позиции вершин
            glm::vec3 position;
            ss >> position.x >> position.y >> position.z;
            positions.push_back(position);
        }
        else if (prefix == "vt") { // Текстурные координаты
            glm::vec2 texCoord;
            ss >> texCoord.x >> texCoord.y;
            texCoords.push_back(texCoord);
        }
        else if (prefix == "vn") { // Нормали
            glm::vec3 normal;
            ss >> normal.x >> normal.y >> normal.z;
            normals.push_back(normal);
        }
        else if (prefix == "f") { // Индексы (face)
            unsigned int posIndex[3] = { 0 }, texIndex[3] = { 0 }, normIndex[3] = { 0 };
            char slash;

            for (int i = 0; i < 3; i++) 
            {
                std::string vertexData;
                ss >> vertexData; // Считываем весь блок данных (например, "1/1/1")

                std::stringstream vertexStream(vertexData);
                std::string pos, tex, norm;

                // Разделяем по символу '/'
                std::getline(vertexStream, pos, '/');
                std::getline(vertexStream, tex, '/');
                std::getline(vertexStream, norm, '/');

                // Преобразуем индексы, если они существуют
                posIndex[i] = !pos.empty() ? std::stoi(pos) : 0;
                texIndex[i] = !tex.empty() ? std::stoi(tex) : 0;
                normIndex[i] = !norm.empty() ? std::stoi(norm) : 0;

                // Проверка индексов позиции
                glm::vec3 position = (posIndex[i] > 0 && posIndex[i] - 1 < positions.size())
                    ? positions[posIndex[i] - 1]
                    : glm::vec3(0.0f);

                // Проверка индексов текстурных координат
                glm::vec2 texCoord = (texIndex[i] > 0 && texIndex[i] - 1 < texCoords.size())
                    ? texCoords[texIndex[i] - 1]
                    : glm::vec2(0.0f);

                // Проверка индексов нормалей
                glm::vec3 normal = (normIndex[i] > 0 && normIndex[i] - 1 < normals.size())
                    ? normals[normIndex[i] - 1]
                    : glm::vec3(0.0f, 0.0f, 1.0f);

                //std::cout << "Face: ";
                //std::cout << "v: " << posIndex[i] << " vt: " << texIndex[i] << " vn: " << normIndex[i] << std::endl;

                // Добавление векторных данных в массив для OpenGL
                loadedVertices.insert(loadedVertices.end(), 
                    {
                    position.x, position.y, position.z,
                    normal.x, normal.y, normal.z,
                    texCoord.x, texCoord.y
                    });
            }
        }

    }
    objFile.close();
    std::cout << "Successfully loaded .obj file: " << filePath << std::endl;
    return loadedVertices;
}