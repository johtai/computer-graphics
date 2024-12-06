#include "shapes.h"



//TODO ИСПРАВИТЬ ОШИБКУ НАРУШЕНИЯ ДОСТУПА К ЧТЕНИЮ

const char* VertexShaderSource = R"(
	        #version 330 core
            uniform vec3 translation;
	        in vec3 coord;
	        in vec3 colorVertex; //Цвет вершины (для градиента)
	        out vec3 vertexColor; //Передача цвета в фрагментный шейдер

	        void main() {
		        gl_Position = vec4(coord.x * scale.x, coord.y * scale.y, 0.0, 1.0);
		        vertexColor = colorVertex;
	        }
        )";

const char* FragmentGradientShaderSource = R"(
	        #version 330 core
            in vec3 vertexColor;
            out vec4 color;

            void main()
            {
                color = vec4(vertexColor, 1.0);
            }
        )";

const char* FragmentTextureShaderSource = R"(
	        #version 330 core
            in vec3 vertexColor;
            out vec4 color;

            void main()
            {
                color = vec4(vertexColor, 1.0);
            }
        )";

//const char* UniformFragmentShaderSource = R"(
//	        #version 330 core
//            uniform vec3 uniformcolor;
//            out vec4 color;
//
//            void main()
//            {
//                color = vec4(uniformcolor, 1.0);
//            }
//        )";

// Компиляция шейдера
GLuint compileShader(const GLchar* source, GLenum type) {
    GLuint shader = glCreateShader(type);
    glShaderSource(shader, 1, &source, nullptr);
    glCompileShader(shader);

    return shader;
}

// Создание шейдера
GLuint createShaderProgram(int shaderindex) {
    GLuint fragmentShader = compileShader(FragmentGradientShaderSource, GL_FRAGMENT_SHADER);
    GLuint vertexShader = compileShader(VertexShaderSource, GL_VERTEX_SHADER);


    //Градиентная фигура
    if (shaderindex == 0)
    {
        fragmentShader = compileShader(FragmentGradientShaderSource, GL_FRAGMENT_SHADER);
    }
    //Куб с текстурой
    else if (shaderindex == 1)
    {
        fragmentShader = compileShader(FragmentTextureShaderSource, GL_FRAGMENT_SHADER);
    }

    GLuint shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);

    return shaderProgram;
}

// Создает фигуру в VBO по вершинам
void getShape(GLuint& VBO, GLuint count) {
    glGenBuffers(1, &VBO);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);

    if (count == 0)
        glBufferData(GL_ARRAY_BUFFER, cubeVertices.size() * sizeof(GLfloat), cubeVertices.data(), GL_STATIC_DRAW);
    else if (count == 1)
        glBufferData(GL_ARRAY_BUFFER, tetrahedronVertices.size() * sizeof(GLfloat), tetrahedronVertices.data(), GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
}

//GLuint loadTexture(const std::string& filepath) 
//{
//    sf::Image image;
//    if (!image.loadFromFile(filepath))
//    {
//        std::cerr << "Failed to load texture1.png" << std::endl;
//        return -1;
//    }
//
//    GLuint textureID;
//    glGenTextures(1, &textureID);
//    glBindTexture(GL_TEXTURE_2D, textureID);
//
//    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, image.getSize().x, image.getSize().y, 0, GL_RGBA, GL_UNSIGNED_BYTE, image.getPixelsPtr());
//
//    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
//    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
//
//    glBindTexture(GL_TEXTURE_2D, 0); // Отключаем текстуру
//
//    return textureID;
//
//}

int main() {
    
    sf::RenderWindow window(sf::VideoMode(500, 500), "Window");
    glewInit();

    GLuint VBO;
    GLuint count = 0; 

    GLuint shaderProgram = createShaderProgram(count);
    getShape(VBO, count);

    while (window.isOpen()) 
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed) { window.close(); }
            else if (event.type == sf::Event::MouseButtonPressed) 
            {
                count = (count + 1) % 2;
                shaderProgram = createShaderProgram(count % 2);
                if (count == 0 || count == 1)
                    getShape(VBO, count);
            }
        }

        glClear(GL_COLOR_BUFFER_BIT);

        glUseProgram(shaderProgram);

        //GLuint texture = loadTexture("texture1.png");
        //glBindTexture(GL_TEXTURE_2D, texture);
        // Считываем координаты x, y, z и передаем по индексу 0
        glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)0); // 3 координаты
        glEnableVertexAttribArray(0);

        // Считываем цвета r, g, b и передаем по индексу 1
        glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat))); // Öâåò
        glEnableVertexAttribArray(1);

        // uniform - для любых двух шейдеров
        // attrib - для вершинного

        // Полуаем адрес для хранения цвета в uniform
        //GLuint colorLocation = glGetUniformLocation(shaderProgram, "uniformcolor");
        //glUniform3f(colorLocation, 1.0f, 0.0f, 0.0f);

        glBindBuffer(GL_ARRAY_BUFFER, NULL);

        // Куб
        if (count == 0)
            glDrawArrays(GL_QUADS, 0, 12);
        // Тетраэдр
        else if (count == 1)
            glDrawArrays(GL_TRIANGLES, 0, 12);

        window.display();
    }
    glBindTexture(GL_TEXTURE_2D, 0);
    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);

    return 0;
}