import { Component, inject } from '@angular/core';
import { QuizService } from '../../core/services/quiz.service';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-quiz-taker',
  imports: [RouterLink],
  templateUrl: './quiz-taker.component.html',
  styleUrl: './quiz-taker.component.scss',
})
export class QuizTakerComponent {
  quizService = inject(QuizService);
  private route = inject(ActivatedRoute);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.quizService.startQuiz(id);
    }
  }

  isOptionSelected(questionId: string, option: string): boolean {
    const savedAnswer = this.quizService.userAnswers()[questionId];
    if (!savedAnswer) return false;
    if (Array.isArray(savedAnswer)) {
      return savedAnswer.includes(option);
    }

    return savedAnswer === option;
  }

  onRadioChange(questionId: string, option: string) {
    this.quizService.answerQuestion(questionId, option);
  }

  onCheckboxChange(questionId: string, option: string, isChecked: boolean) {
    const currentAnswer = this.quizService.userAnswers()[questionId];
    const currentList = Array.isArray(currentAnswer) ? currentAnswer : [];

    let newList: string[];

    if (isChecked) {
      newList = [...currentList, option];
    }
    else {
      newList = currentList.filter(item => item !== option);
    }

    this.quizService.answerQuestion(questionId, newList);
  }
}
