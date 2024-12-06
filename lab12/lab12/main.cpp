#include <GL/glew.h>
#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>

// Квадрат
const std::vector<GLfloat> quadVertices = {
    -0.5f, -0.5f,     1.0f, 0.0f, 0.0f,
     0.5f, -0.5f,     1.0f, 0.0f, 0.0f,
     0.5f,  0.5f,     0.0f, 1.0f, 1.0f,
    -0.5f,  0.5f,     0.0f, 1.0f, 1.0f
};

// Веер
const std::vector<GLfloat> fanVertices = {
      0.0f, 0.0f,    1.0f, 0.0f, 0.0f,
     -0.6f, 0.2f,    1.0f, 0.0f, 0.0f,
     -0.4f, 0.4f,    1.0f, 0.0f, 0.0f,
      0.0f, 0.5f,    0.0f, 1.0f, 1.0f,
      0.5f, 0.5f,    0.0f, 1.0f, 1.0f,
      0.8f, 0.0f,    0.0f, 1.0f, 1.0f
};

// Пятиугольник
const std::vector<GLfloat> pentaVertices = {
     0.0f,  1.0f,  1.0f, 0.0f, 0.0f,
     1.0f,  0.3f,  1.0f, 0.0f, 0.0f,
     0.6f, -0.8f,  0.0f, 1.0f, 1.0f,
    -0.6f, -0.8f,  0.0f, 1.0f, 1.0f,
    -1.0f,  0.3f,  0.0f, 1.0f, 1.0f,
};

const char* VertexShaderSource = R"(
	        #version 330 core
	        in vec2 coord;
	        in vec3 colorVertex; //Цвет вершины (для градиента)
	        out vec3 vertexColor; //Передача цвета в фрагментный шейдер

	        void main() {
		        gl_Position = vec4(coord, 0.0, 1.0); 
		        vertexColor = colorVertex;
	        }
        )";

const char* ConstFragmentShaderSource = R"(
	        #version 330 core
            out vec4 color;

            void main()
            {
                color = vec4(0.0, 1.0, 1.0, 1.0);
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

const char* UniformFragmentShaderSource = R"(
	        #version 330 core
            uniform vec3 uniformcolor;
            out vec4 color;

            void main()
            {
                color = vec4(uniformcolor, 1.0);
            }
        )";

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

    if (shaderindex == 0)
    {
        fragmentShader = compileShader(ConstFragmentShaderSource, GL_FRAGMENT_SHADER);
    }
    else if (shaderindex == 1)
    {
        fragmentShader = compileShader(UniformFragmentShaderSource, GL_FRAGMENT_SHADER);
    }
    else if (shaderindex == 2)
    {
        fragmentShader = compileShader(FragmentGradientShaderSource, GL_FRAGMENT_SHADER);
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
        glBufferData(GL_ARRAY_BUFFER, quadVertices.size() * sizeof(GLfloat), quadVertices.data(), GL_STATIC_DRAW);
    else if (count == 3)
        glBufferData(GL_ARRAY_BUFFER, fanVertices.size() * sizeof(GLfloat), fanVertices.data(), GL_STATIC_DRAW);
    else
        glBufferData(GL_ARRAY_BUFFER, pentaVertices.size() * sizeof(GLfloat), pentaVertices.data(), GL_STATIC_DRAW);

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
}

int main() {
    sf::RenderWindow window(sf::VideoMode(500, 500), "Window");
    glewInit();

    GLuint VBO;
    GLuint count = 0;

    GLuint shaderProgram = createShaderProgram(count % 3);
    getShape(VBO, count);

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed) { window.close(); }
            else if (event.type == sf::Event::MouseButtonPressed) {
                count = (count + 1) % 9;
                shaderProgram = createShaderProgram(count % 3);
                if (count == 0 || count == 3 || count == 6)
                    getShape(VBO, count);
            }
        }

        glClear(GL_COLOR_BUFFER_BIT);

        glUseProgram(shaderProgram);

        glBindBuffer(GL_ARRAY_BUFFER, VBO);

        // Считываем координаты x, y и передаем по индексу 0
        glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)0); // Ïîçèöèÿ
        glEnableVertexAttribArray(0);

        // Считываем цвета r, g, b и передаем по индексу 1
        glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(GLfloat), (void*)(2 * sizeof(GLfloat))); // Öâåò
        glEnableVertexAttribArray(1);

        // uniform - для любых двух шейдеров
        // attrib - для вершинного

        // Полуаем адрес для хранения цвета в uniform
        GLuint colorLocation = glGetUniformLocation(shaderProgram, "uniformcolor");
        glUniform3f(colorLocation, 1.0f, 0.0f, 0.0f);

        glBindBuffer(GL_ARRAY_BUFFER, NULL);

        // Квадрат
        if (count == 0 || count == 1 || count == 2)
            glDrawArrays(GL_QUADS, 0, 4);
        // Веер
        else if (count == 3 || count == 4 || count == 5)
            glDrawArrays(GL_TRIANGLE_FAN, 0, 6);
        // Пятиугольник
        else
            glDrawArrays(GL_POLYGON, 0, 5);

        window.display();
    }

    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);

    return 0;
}