#include <SFML/Graphics.hpp>
#include <GL/glew.h>
#include <iostream>

// ID шейдерной программы
GLuint Program;
// ID атрибута
GLint Attrib_vertex;
// ID Vertex Buffer Object
GLuint VBO;

GLint uniformR, uniformG, uniformB;

struct Vertex {
	GLfloat x;
	GLfloat y;
};

// Вершинный шейдер
// 0.0 - пустой Z, нет объема; 1.0 - альфа канал полный
const char* VertexShaderSource = R"(
	#version 330 core
	in vec2 coord;
	void main() {
		gl_Position = vec4(coord, 0.0, 1.0);
	}
)";
GLint R = 1;
GLint G = 1;
GLint B = 0;

// Исходный код фрагментного (пикселный) шейдера
// R = 0; G = 1; B = 0; Alpha = 1
const char* FragShaderSource = R"(
	#version 330 core
	out vec4 color;
	uniform float R;
	uniform float G;
	uniform float B;
	void main() {
		color = vec4(R, G, B, 1);
	}
)";

void ShaderLog(unsigned int shader)
{
	int infologLen = 0;
	glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
	if (infologLen > 1)
	{
		int charsWritten = 0;
		std::vector<char> infoLog(infologLen);
		glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog.data());
		std::cout << "InfoLog: " << infoLog.data() << std::endl;
	}
}

// Освобождение буфера
void ReleaseVBO() {
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glDeleteBuffers(1, &VBO);
}

// Освобождение шейдеров
void ReleaseShader() {
	// Передавая ноль, мы отключаем шейдерную программу
	glUseProgram(0);
	
	// Удаляем шейдерную программу
	glDeleteProgram(Program);
}

void Release() {
	// Шейдеры
	ReleaseShader();
	// Вершинный буфер
	ReleaseVBO();
}

void Draw() {
	glUseProgram(Program); // Устанавливаем шейдерную программу текущей
	glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов
	glBindBuffer(GL_ARRAY_BUFFER, VBO); // Подключаем VBO
	
	// сообщаем OpenGL как он должен интерпретировать вершинные данные.
	// 2 - берем по 2
	// GL_FALSE - нормализация (от 0 до 2)
	// 0 шаг (берем каждый, он сам считает)
	// берем с 0 элемента буфера (с начала)
	glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 0, 0);
	glBindBuffer(GL_ARRAY_BUFFER, 0); // Отключаем VBO

	// начиная с нулевой вершины берем 3
	glDrawArrays(GL_TRIANGLES, 0, 3); // Передаем данные на видеокарту (рисуем)
	glDisableVertexAttribArray(Attrib_vertex); // Отключаем массив атрибутов
	glUseProgram(0); // Отключаем шейдерную программу
	//checkOpenGLerror();
}

void InitShader() {
	// Создаем вершинный шейдер
	GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
	
	// Передаем исходный код
	glShaderSource(vShader, 1, &VertexShaderSource, NULL);
	
	// Компилируем шейдер
	glCompileShader(vShader);
	std::cout << "vertex shader \n";
	
	// Функция печати лога шейдера
	ShaderLog(vShader); //Пример функции есть в лабораторной

	// Создаем фрагментный шейдер
	GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
	
	// Передаем исходный код
	glShaderSource(fShader, 1, &FragShaderSource, NULL);
	
	// Компилируем шейдер
	glCompileShader(fShader);
	std::cout << "fragment shader \n";
	
	// Функция печати лога шейдера
	ShaderLog(fShader);

	// Создаем программу и прикрепляем шейдеры к ней
	Program = glCreateProgram();
	glAttachShader(Program, vShader);
	glAttachShader(Program, fShader);
	
	// Линкуем шейдерную программу
	glLinkProgram(Program);
	
	uniformR = glGetUniformLocation(Program, "R");
	uniformG = glGetUniformLocation(Program, "G");
	uniformB = glGetUniformLocation(Program, "B");

	std::cout << uniformG;
	if (uniformR == -1 || uniformG == -1 || uniformB == -1) 
	{
		std::cerr << "not init uniform shaders" << std::endl;
	}


	// Проверяем статус сборки
	int link_ok;
	glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
	if (!link_ok) {
		std::cout << "error attach shaders \n";
		return;
	}

	// Вытягиваем ID атрибута из собранной программы
	const char* attr_name = "coord"; //имя в шейдере
	Attrib_vertex = glGetAttribLocation(Program, attr_name);
	if (Attrib_vertex == -1) {
		std::cout << "could not bind attrib " << attr_name << std::endl;
		return;
	}
	//checkOpenGLerror();
}

void UpdateUniforms() {
	glUseProgram(Program);
	glUniform1f(uniformR, R);
	glUniform1f(uniformG, G);
	glUniform1f(uniformB, B);
	glUseProgram(0);
}

struct  figures 
{

	Vertex triangle[3] = {
	{ -1.0f, -1.0f },
	{ 0.0f, 1.0f },
	{ 1.0f, -1.0f }
	};

};


void InitVBO() {
	glGenBuffers(1, &VBO);
	figures fig;
	//// Вершины нашего треугольника
	//Vertex triangle[3] = {
	//	{ -1.0f, -1.0f },
	//	{ 0.0f, 1.0f },
	//	{ 1.0f, -1.0f }
	//};

	Vertex*  BufferFigure = fig.triangle;

	// Передаем вершины в буфер
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(fig.triangle), BufferFigure, GL_STATIC_DRAW);
	//checkOpenGLerror(); //Пример функции есть в лабораторной
	// Проверка ошибок OpenGL, если есть, то вывод в консоль тип ошибки
}

void Init() {
	// Шейдеры
	InitShader();
	// Вершинный буфер, VBO (Vertex Buffer Object) - информация о вершинах
	InitVBO();
}

int main() {
	sf::Window window(sf::VideoMode(600, 600), "My OpenGL window", sf::Style::Default, sf::ContextSettings(24));
	window.setVerticalSyncEnabled(true);
	window.setActive(true);

	glewInit();
	// Шейдеры и вершинный буфер
	Init();
	
	while (window.isOpen()) {
		sf::Event event;
		while (window.pollEvent(event)) {
			if (event.type == sf::Event::Closed) { window.close(); }
			else if (event.type == sf::Event::Resized) { glViewport(0, 0, event.size.width, event.size.height); }
		}
		
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		UpdateUniforms();
		Draw();
		window.display();
	}
	
	// Шейдеры и вершинный буфер
	Release();
	return 0;
}