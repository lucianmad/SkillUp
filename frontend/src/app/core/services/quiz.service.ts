import { Injectable, signal } from '@angular/core';
import { Quiz } from '../../shared/models/quiz.model';
import { QuestionType } from '../../shared/enums/question-type.enum';

@Injectable({
  providedIn: 'root',
})
export class QuizService {
  private mockQuizzes: Quiz[] = 
  [
    {
      id: 'quiz-angular',
      title: 'Angular Foundations',
      questions: [
        {
          id: 'q1',
          text: 'What is the purpose of a Signal in Angular?',
          type: QuestionType.SingleChoice,
          options: ['To handle HTTP requests', 'To manage reactive state', 'To styling components'],
          correctAnswers: ['To manage reactive state']
        },
        {
          id: 'q2',
          text: 'Which file is used for global styles?',
          type: QuestionType.SingleChoice,
          options: ['app.component.ts', 'styles.scss', 'index.html'],
          correctAnswers: ['styles.scss']
        }
      ]
    },
    {
      id: 'quiz-csharp',
      title: 'C# & .NET Mastery',
      questions: [
        {
          id: 'q3',
          text: 'Which keyword makes a class un-inheritable?',
          type: QuestionType.SingleChoice,
          options: ['static', 'sealed', 'private'],
          correctAnswers: ['sealed']
        },
        {
          id: 'q4',
          text: 'What are valid Reference Types in C#?',
          type: QuestionType.MultipleChoice,
          options: ['Class', 'Interface', 'int', 'bool'],
          correctAnswers: ['Class', 'Interface']
        }
      ]
    },
    {
      id: 'quiz-postgres',
      title: 'PostgreSQL Basics',
      questions: [
        {
          id: 'q5',
          text: 'Which command removes all data but keeps the table structure?',
          type: QuestionType.SingleChoice,
          options: ['DROP TABLE', 'DELETE ALL', 'TRUNCATE'],
          correctAnswers: ['TRUNCATE']
        },
        {
          id: 'q6',
          text: 'Which data type is best for storing JSON documents?',
          type: QuestionType.SingleChoice,
          options: ['TEXT', 'JSONB', 'VARCHAR'],
          correctAnswers: ['JSONB']
        }
      ]
    },
    {
      id: 'quiz-arch',
      title: 'Full Stack Architecture',
      questions: [
        {
          id: 'q7',
          text: 'Where should business logic live?',
          type: QuestionType.SingleChoice,
          options: ['In the Database', 'In the Backend API', 'In the HTML'],
          correctAnswers: ['In the Backend API']
        },
        {
          id: 'q8',
          text: 'What is the benefit of a "Smart" component?',
          type: QuestionType.SingleChoice,
          options: ['It looks pretty', 'It fetches data and manages state', 'It is reusable everywhere'],
          correctAnswers: ['It fetches data and manages state']
        }
      ]
    }
  ];

  readonly quizzes = signal<Quiz[]>([]);
  readonly activeQuiz = signal<Quiz | null>(null);


  constructor() {
    this.loadQuizzes();
  }

  private loadQuizzes() {
    this.quizzes.set(this.mockQuizzes);
  }
}
