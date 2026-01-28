import { computed, Injectable, signal } from '@angular/core';
import { Quiz } from '../../shared/models/quiz.model';
import { QuestionType } from '../../shared/enums/question-type.enum';
import { Question } from '../../shared/models/question.model';

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
  readonly currentQuestionIndex = signal<number>(0);
  readonly userAnswers = signal<Record<string, string | string[]>>({});

  readonly currentQuestion = computed(() => {
    const quiz = this.activeQuiz();
    const index = this.currentQuestionIndex();
    return quiz ? quiz.questions[index] : null;
  });

  readonly isLastQuestion = computed(() => {
    const quiz = this.activeQuiz();
    if (!quiz) return false;
    return this.currentQuestionIndex() === quiz.questions.length - 1;
  });

  constructor() {
    this.loadQuizzes();
  }

  private loadQuizzes() {
    this.quizzes.set(this.mockQuizzes);
  }

  startQuiz(quizId: string) {
    const quiz = this.mockQuizzes.find(q => q.id === quizId);
    if (quiz) {
      this.activeQuiz.set(quiz);
      this.currentQuestionIndex.set(0);
      this.userAnswers.set({});
    }
  }

  answerQuestion(questionId: string, answer: string | string[]) {
    this.userAnswers.update(prev => ({
      ...prev,
      [questionId]: answer
    }));
  }

  nextQuestion() {
    if (!this.isLastQuestion()) {
      this.currentQuestionIndex.update(index => index + 1);
    }
  }

  prevQuestion() {
    if (this.currentQuestionIndex() >= 1) {
      this.currentQuestionIndex.update(index => index - 1);
    }
  }

  computeResults(quizId: string) {
    let score = 0;
    const quiz = this.quizzes().find(quiz => quiz.id === quizId);
    if (quiz) {
      for (const question of quiz.questions) {
        if (question.type === QuestionType.SingleChoice) {
          if (this.computeResultsForSingleChoice(question, this.userAnswers())) {
            score++;
          }
        }
        else if (question.type === QuestionType.MultipleChoice) {
          if (this.computeResultsForMultipleChoice(question, this.userAnswers())){
            score++;
          }
        }
      }
    }

    return score;
  }

  private computeResultsForSingleChoice(question: Question, answers: Record<string, string | string[]>) {
    const userAnswers = answers[question.id];
    if (!userAnswers) {
      return false;
    }

    return question.correctAnswers.includes(userAnswers as string);
  }

  private computeResultsForMultipleChoice(question: Question, answers: Record<string, string | string[]>) {
    const userAnswers = answers[question.id] as string[];
    const correct = question.correctAnswers;
    
    if (!userAnswers || userAnswers.length !== correct.length) {
      return false;
    }
    
    return userAnswers.every(item => correct.includes(item));
  }  
}
