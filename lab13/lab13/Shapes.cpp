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


std::vector<GLfloat> loadedVertices; // Хранение загруженных данных

void ParseObjFromFile(const std::string& filePath) {
    std::ifstream objFile(filePath);
    if (!objFile.is_open()) {
        std::cerr << "Failed to open .obj file: " << filePath << std::endl;
        return;
    }

    std::vector<glm::vec3> positions;
    std::vector<glm::vec2> texCoords;
    std::vector<glm::vec3> normals;

    loadedVertices.clear();

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
            unsigned int posIndex[3], texIndex[3], normIndex[3];
            char slash; // Для пропуска символа '/' в индексе
            for (int i = 0; i < 3; i++) {
                ss >> posIndex[i] >> slash >> texIndex[i] >> slash >> normIndex[i];
                glm::vec3 position = positions[posIndex[i] - 1];
                glm::vec2 texCoord = texCoords[texIndex[i] - 1];
                glm::vec3 normal = normals[normIndex[i] - 1];

                // Добавление векторных данных в массив для OpenGL
                loadedVertices.insert(loadedVertices.end(), {
                    position.x, position.y, position.z,
                    normal.x, normal.y, normal.z,
                    texCoord.x, texCoord.y
                    });
            }
        }
    }
    objFile.close();
    std::cout << "Successfully loaded .obj file: " << filePath << std::endl;
}
