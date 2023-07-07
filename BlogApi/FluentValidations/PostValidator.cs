using BlogApi.Entities;
using BlogApi.Models.PostModels;
using FluentValidation;

namespace BlogApi.FluentValidations;

public class PostValidator : AbstractValidator<CreatePostModel>
{
    public PostValidator()
    {
   
        RuleFor(title => title.Title).NotNull();
        RuleFor(content => content.Content).NotNull();

        When(image => image.Image != null, () =>
        {
            RuleFor(tag => tag.Tag).NotNull().Length(0, 250).NotEqual("Yes image!");
        }).Otherwise(() =>
        {
            RuleFor(tag => tag.Tag).NotNull().Length(0, 250).NotEqual("No image!");
        });
    }
}